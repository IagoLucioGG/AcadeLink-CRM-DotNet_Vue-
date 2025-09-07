using Microsoft.EntityFrameworkCore;
using CRM.Data;
using CRM.Models.Empresa_;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;



namespace CRM.Services.Util_
{
    public class Util
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public Util(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> VerificaEmpresa(string idEmpresa)
        {
            int id = int.Parse(idEmpresa);
            return await _context.Empresas
                .AnyAsync(e => e.IdEmpresa == id && e.Ativo);
        }

        public async Task<Empresa> BuscaEmpresaAsync(int idEmpresa)
        {


            var empresa = await _context.Empresas
                .FirstOrDefaultAsync(e => e.IdEmpresa == idEmpresa && e.Ativo);

            if (empresa == null)
                throw new Exception("Empresa não encontrada ou inativa na base de dados.");

            return empresa;
        }


        public async Task<int> GetIdEmpresaFromToken()
        {
            var claim = _httpContextAccessor.HttpContext?.User.FindFirst("idEmpresa")?.Value;

            if (string.IsNullOrEmpty(claim))
                throw new Exception("IdEmpresa não encontrado no token.");

            if (!int.TryParse(claim, out int idEmpresa))
                throw new Exception("IdEmpresa inválido no token.");

            var empresa = await VerificaEmpresa(claim);
            if (!empresa)
                throw new Exception("Empresa não existe no banco de dados ou não está ativa.");

            return idEmpresa;
        }

        public int GetIdUsuarioFromToken()
        {
            var claim = _httpContextAccessor.HttpContext?.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(claim))
                throw new Exception("IdUsuario não encontrado no token.");

            if (!int.TryParse(claim, out int idUsuario))
                throw new Exception("IdUsuario inválido no token.");

            return idUsuario;
        }


    }
}