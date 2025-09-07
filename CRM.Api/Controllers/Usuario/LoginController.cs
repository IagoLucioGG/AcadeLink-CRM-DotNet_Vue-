using CRM.Models.ResponseModel;
using CRM.Models.Usuario_;
using CRM.DTOs.Usuario_;
using CRM.Services.Usuario_;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CRM.Services.Util_;

namespace CRM.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly Util _util;

        public LoginController(IUsuarioService usuarioService, Util util)
        {
            _usuarioService = usuarioService;
            _util = util;
        }

        /// <summary>
        /// Realiza login e retorna um token JWT junto com os dados do usuário.
        /// </summary>
        /// 
        [AllowAnonymous]
        [HttpPost]

        public async Task<ActionResult<ResponseModel<LoginResponseDto>>> Login([FromBody] LoginRequestDto loginDto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(loginDto.Login) || string.IsNullOrWhiteSpace(loginDto.Senha))
                    return BadRequest(ResponseModel<LoginResponseDto>.Erro("Login e senha são obrigatórios."));

                var usuario = await _usuarioService.LoginAsync(loginDto.Login, loginDto.Senha);

                if (usuario == null)
                    return Unauthorized(ResponseModel<LoginResponseDto>.Erro("Usuário ou senha inválidos."));

                var token = _usuarioService.GerarToken(usuario);
                var empresa = await _util.BuscaEmpresaAsync(usuario.IdEmpresa);

                var sessao = await _usuarioService.SalvarSessao(usuario.IdUsuario, usuario.IdEmpresa, token);
                if (!sessao)
                    return BadRequest(ResponseModel<LoginResponseDto>.Erro("Algo deu errado ao salvar a sessão"));

                var resposta = new LoginResponseDto
                {
                    Usuario = usuario,
                    Token = token,
                    Empresa = empresa
                };


                return Ok(ResponseModel<LoginResponseDto>.Sucesso(resposta, "Login realizado com sucesso."));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseModel<LoginResponseDto>.Erro(ex.Message));
            }

        }


        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            int idUsuario;

            try
            {
                idUsuario = _util.GetIdUsuarioFromToken();
            }
            catch (Exception ex)
            {
                return Unauthorized(ResponseModel<string>.Erro(ex.Message));
            }

            var sucesso = await _usuarioService.LogoutAsync(idUsuario);

            if (!sucesso)
                return NotFound(ResponseModel<string>.Erro("Sessão não encontrada."));

            return Ok(ResponseModel<string>.Sucesso("Logout realizado com sucesso."));
        }



    }
}
