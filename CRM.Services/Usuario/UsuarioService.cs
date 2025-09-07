using CRM.Data;
using CRM.Maps;
using CRM.Models.Usuario_;
using CRM.DTOs.Usuario_;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using CRM.Services.Util_;
using CRM.Models.ResponseModel;


namespace CRM.Services.Usuario_
{
    public class UsuarioService : IUsuarioService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly Util _util;

        public UsuarioService(AppDbContext context, IConfiguration configuration, Util util)
        {
            _context = context;
            _configuration = configuration;
            _util = util;
        }

        public async Task<UsuarioRetornoDto> CadastrarUsuarioAsync(UsuarioCadastroDto dto)
        {
            var idEmpresa = await _util.GetIdEmpresaFromToken();
            var usuarioExistente = await _context.Usuarios.FirstOrDefaultAsync(u => u.NmLogin == dto.NmLogin && u.IdEmpresa == idEmpresa);
            if (usuarioExistente != null)
                throw new Exception("Já existe um Usuário com esse Login");

            var usuario = MapeadorModels.Montar<Usuario, UsuarioCadastroDto>(dto);
            usuario.Senha = HashSenha(usuario.Senha);
            usuario.DataCriacao = DateTime.Now;
            usuario.IdEmpresa = await _util.GetIdEmpresaFromToken();

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return MapeadorModels.Montar<UsuarioRetornoDto, Usuario>(usuario);
        }

        public async Task<UsuarioRetornoDto?> LoginAsync(string login, string senha)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.NmLogin == login && u.Ativo);

            if (usuario == null) return null;

            string idEmpresaStr = usuario.IdEmpresa.ToString();
            var empresaValida = await _util.VerificaEmpresa(idEmpresaStr);
            if (!empresaValida) return null;

            var senhaHash = HashSenha(senha);
            if (usuario.Senha != senhaHash) return null;

            // 🔹 Verificar limite
            var empresa = await _context.Empresas.FindAsync(int.Parse(idEmpresaStr));
            if (empresa == null) return null;
            int limite = empresa.QuantUsuarios;

            int sessoesAtivas = await _context.SessoesUsuarios
                .CountAsync(s => s.IdEmpresa == usuario.IdEmpresa);

            if (sessoesAtivas >= limite)
                throw new Exception("Limite de usuários simultâneos atingido para esta empresa.");

            return MapeadorModels.Montar<UsuarioRetornoDto, Usuario>(usuario);
        }

        public async Task<bool> SalvarSessao(int idUsuario, int idEmpresa, string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                var dataExpiracao = jwtToken.ValidTo;

                var sessao = new SessaoUsuario
                {
                    IdUsuario = idUsuario,
                    IdEmpresa = idEmpresa,
                    Token = token,
                    DataCriacao = DateTime.Now,
                    DataExpiracao = dataExpiracao,
                    UltimaRequisicao = DateTime.Now
                };

                _context.SessoesUsuarios.Add(sessao);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> LogoutAsync(int idUsuario)
        {
            var sessao = await _context.SessoesUsuarios
                .FirstOrDefaultAsync(s => s.IdUsuario == idUsuario);

            if (sessao == null)
                return false;

            _context.SessoesUsuarios.Remove(sessao);
            await _context.SaveChangesAsync();
            return true;
        }



        public async Task<UsuarioRetornoDto?> EditarUsuarioAsync(UsuarioEdicaoDto dto)
        {
            var idEmpresa = await _util.GetIdEmpresaFromToken();
            var usuarioExistente = await _context.Usuarios.FirstOrDefaultAsync(u => u.NmLogin == dto.NmLogin && u.IdEmpresa == idEmpresa);
            if (usuarioExistente != null)
                throw new Exception("Já existe um Usuário com esse Login");

            var usuarioDb = await _context.Usuarios.FindAsync(dto.IdUsuario);
            if (usuarioDb == null) return null;

            MapeadorModels.CopiarPropriedades(dto, usuarioDb);

            if (!string.IsNullOrWhiteSpace(dto.Senha))
                usuarioDb.Senha = HashSenha(dto.Senha);

            await _context.SaveChangesAsync();
            return MapeadorModels.Montar<UsuarioRetornoDto, Usuario>(usuarioDb);
        }

        public async Task<bool> AlterarStatusUsuarioAsync(int idUsuario, bool ativo)
        {
            var usuario = await _context.Usuarios.FindAsync(idUsuario);
            if (usuario == null) return false;

            usuario.Ativo = ativo;
            await _context.SaveChangesAsync();
            return true;
        }

        private string HashSenha(string senha)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(senha));
            return Convert.ToBase64String(bytes);
        }

        public string GerarToken(UsuarioRetornoDto usuario)
        {
            var chaveSecreta = _configuration["Jwt:Chave"] ?? "Chave-Secreta-de-32-Caracteres-Super-Dificil";
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(chaveSecreta));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var dataGeracao = DateTime.Now;
            var dataExpiracao = dataGeracao.AddHours(2);

            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, usuario.IdUsuario.ToString()),
        new Claim(JwtRegisteredClaimNames.UniqueName, usuario.NmLogin),
        new Claim("idEmpresa", usuario.IdEmpresa.ToString()), // ✅ Aqui vai o IdEmpresa
        new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(dataGeracao).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
        new Claim("dataGeracao", dataGeracao.ToString("yyyy-MM-dd HH:mm:ss")),
        new Claim("dataExpiracao", dataExpiracao.ToString("yyyy-MM-dd HH:mm:ss"))
    };

            var token = new JwtSecurityToken(
                claims: claims,
                notBefore: dataGeracao,
                expires: dataExpiracao,
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }



        public async Task<bool> AlterarSenhaAsync(string login, string senhaAntiga, string novaSenha)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.NmLogin == login && u.Ativo);
            if (usuario == null)
                return false;

            var senhaAntigaHash = HashSenha(senhaAntiga);
            if (usuario.Senha != senhaAntigaHash)
                return false;

            usuario.Senha = HashSenha(novaSenha);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<UsuarioRetornoDto>> ListarUsuariosAsync()
        {
            int idEmpresa = await _util.GetIdEmpresaFromToken();

            return await _context.Usuarios
                .Where(u => u.IdEmpresa == idEmpresa)
                .Select(u => new UsuarioRetornoDto
                {
                    IdUsuario = u.IdUsuario,
                    NmAgente = u.NmAgente,
                    NmLogin = u.NmLogin,
                    Ativo = u.Ativo,
                    IdEmpresa = u.IdEmpresa
                })
                .ToListAsync();
        }


    }
}
