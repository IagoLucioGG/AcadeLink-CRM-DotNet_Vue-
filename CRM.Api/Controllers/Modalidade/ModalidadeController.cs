using CRM.DTOs.Modalidade_;
using CRM.Models.ResponseModel;
using CRM.Services.Modalidade_;
using Microsoft.AspNetCore.Mvc;

namespace CRM.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ModalidadeController : ControllerBase
    {
        private readonly IModalidadeService _modalidadeService;

        public ModalidadeController(IModalidadeService modalidadeService)
        {
            _modalidadeService = modalidadeService;
        }

        [HttpPost("cadastrar")]
        public async Task<ActionResult<ResponseModel<ModalidadeRetornoDto>>> Cadastrar([FromBody] ModalidadeCadastroDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.NmModalidade))
                return BadRequest(ResponseModel<ModalidadeRetornoDto>.Erro("Nome da modalidade é obrigatório."));

            var modalidade = await _modalidadeService.CadastrarModalidadeAsync(dto);
            return Ok(ResponseModel<ModalidadeRetornoDto>.Sucesso(modalidade, "Modalidade cadastrada com sucesso."));
        }

        [HttpPut("editar")]
        public async Task<ActionResult<ResponseModel<ModalidadeRetornoDto>>> Editar([FromBody] ModalidadeEdicaoDto dto)
        {
            var modalidade = await _modalidadeService.EditarModalidadeAsync(dto);
            if (modalidade == null)
                return NotFound(ResponseModel<ModalidadeRetornoDto>.Erro("Modalidade não encontrada."));

            return Ok(ResponseModel<ModalidadeRetornoDto>.Sucesso(modalidade, "Modalidade editada com sucesso."));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseModel<ModalidadeRetornoDto>>> BuscarPorId(int id)
        {
            var modalidade = await _modalidadeService.BuscarPorIdAsync(id);
            if (modalidade == null)
                return NotFound(ResponseModel<ModalidadeRetornoDto>.Erro("Modalidade não encontrada."));

            return Ok(ResponseModel<ModalidadeRetornoDto>.Sucesso(modalidade));
        }

        [HttpGet("listar")]
        public async Task<ActionResult<ResponseModel<List<ModalidadeRetornoDto>>>> ListarTodos()
        {
            var lista = await _modalidadeService.ListarTodasAsync();
            return Ok(ResponseModel<List<ModalidadeRetornoDto>>.Sucesso(lista));
        }
    }
}
