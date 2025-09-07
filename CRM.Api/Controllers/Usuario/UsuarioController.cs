using CRM.Models.ResponseModel;
using CRM.Models.Usuario_;
using CRM.DTOs.Usuario_;
using CRM.Services.Usuario_;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace CRM.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        /// <summary>
        /// Cadastrar um novo usuário
        /// </summary>
        [AllowAnonymous]
        [HttpPost("cadastrar")]
        public async Task<ActionResult<ResponseModel<UsuarioRetornoDto>>> Cadastrar([FromBody] UsuarioCadastroDto dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.NmLogin) || string.IsNullOrWhiteSpace(dto.Senha))
                    return BadRequest(ResponseModel<UsuarioRetornoDto>.Erro("Login e senha são obrigatórios."));

                var usuario = await _usuarioService.CadastrarUsuarioAsync(dto);
                return Ok(ResponseModel<UsuarioRetornoDto>.Sucesso(usuario, "Usuário cadastrado com sucesso."));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseModel<UsuarioCadastroDto>.Erro(ex.Message));
            }

        }

        /// <summary>
        /// Editar um usuário existente
        /// </summary>
        [HttpPut("editar")]
        public async Task<ActionResult<ResponseModel<UsuarioRetornoDto>>> Editar([FromBody] UsuarioEdicaoDto dto)
        {
            try
            {
                var usuario = await _usuarioService.EditarUsuarioAsync(dto);
                if (usuario == null)
                    return NotFound(ResponseModel<UsuarioRetornoDto>.Erro("Usuário não encontrado."));

                return Ok(ResponseModel<UsuarioRetornoDto>.Sucesso(usuario, "Usuário editado com sucesso."));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseModel<UsuarioCadastroDto>.Erro(ex.Message));
            }

        }

        /// <summary>
        /// Ativar/Inativar um usuário
        /// </summary>
        [HttpPatch("{idUsuario}/status")]
        public async Task<ActionResult<ResponseModel<bool>>> AlterarStatus(int idUsuario, [FromQuery] bool ativo)
        {
            var sucesso = await _usuarioService.AlterarStatusUsuarioAsync(idUsuario, ativo);
            if (!sucesso)
                return NotFound(ResponseModel<bool>.Erro("Usuário não encontrado."));

            var msg = ativo ? "Usuário ativado com sucesso." : "Usuário inativado com sucesso.";
            return Ok(ResponseModel<bool>.Sucesso(true, msg));
        }

        /// <summary>
        /// Alterar a senha de um usuário
        /// </summary>
        [HttpPatch("alterar-senha")]
        public async Task<ActionResult<ResponseModel<bool>>> AlterarSenha([FromBody] AlterarSenhaDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.NmLogin) ||
                string.IsNullOrWhiteSpace(dto.SenhaAntiga) ||
                string.IsNullOrWhiteSpace(dto.NovaSenha))
            {
                return BadRequest(ResponseModel<bool>.Erro("Login, senha antiga e nova senha são obrigatórios."));
            }

            var sucesso = await _usuarioService.AlterarSenhaAsync(dto.NmLogin, dto.SenhaAntiga, dto.NovaSenha);
            if (!sucesso)
                return BadRequest(ResponseModel<bool>.Erro("Login ou senha antiga inválidos."));

            return Ok(ResponseModel<bool>.Sucesso(true, "Senha alterada com sucesso."));
        }

        [HttpGet("listar")]
        public async Task<ActionResult<ResponseModel<IEnumerable<UsuarioRetornoDto>>>> Listar()
        {
            var usuarios = await _usuarioService.ListarUsuariosAsync();
            return Ok(ResponseModel<IEnumerable<UsuarioRetornoDto>>.Sucesso(usuarios));
        }


    }
}
