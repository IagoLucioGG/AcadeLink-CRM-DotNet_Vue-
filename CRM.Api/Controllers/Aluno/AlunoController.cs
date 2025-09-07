using CRM.DTOs.Aluno_;
using CRM.Models.ResponseModel;
using CRM.Services.Aluno_;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRM.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlunoController : ControllerBase
    {
        private readonly IAlunoService _alunoService;

        public AlunoController(IAlunoService alunoService)
        {
            _alunoService = alunoService;
        }

        [HttpPost("cadastrar")]
        public async Task<ActionResult<ResponseModel<AlunoRetornoDto>>> Cadastrar([FromBody] AlunoCadastroDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.NmAluno))
                return BadRequest(ResponseModel<AlunoRetornoDto>.Erro("Nome do aluno é obrigatório."));

            var aluno = await _alunoService.CadastrarAlunoAsync(dto);
            return Ok(ResponseModel<AlunoRetornoDto>.Sucesso(aluno, "Aluno cadastrado com sucesso."));
        }

        [HttpPut("editar")]
        public async Task<ActionResult<ResponseModel<AlunoRetornoDto>>> Editar([FromBody] AlunoEdicaoDto dto)
        {
            var aluno = await _alunoService.EditarAlunoAsync(dto);
            if (aluno == null)
                return NotFound(ResponseModel<AlunoRetornoDto>.Erro("Aluno não encontrado."));

            return Ok(ResponseModel<AlunoRetornoDto>.Sucesso(aluno, "Aluno editado com sucesso."));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseModel<AlunoRetornoDto>>> BuscarPorId(int id)
        {
            var aluno = await _alunoService.BuscarPorIdAsync(id);
            if (aluno == null)
                return NotFound(ResponseModel<AlunoRetornoDto>.Erro("Aluno não encontrado."));

            return Ok(ResponseModel<AlunoRetornoDto>.Sucesso(aluno));
        }

        [HttpGet("listar")]
        public async Task<ActionResult<ResponseModel<List<AlunoRetornoDto>>>> ListarTodos()
        {
            var lista = await _alunoService.ListarTodosAsync();
            return Ok(ResponseModel<List<AlunoRetornoDto>>.Sucesso(lista));
        }
    }
}
