using CRM.DTOs.Curso_;
using CRM.Models.ResponseModel;
using CRM.Services.Curso_;
using Microsoft.AspNetCore.Mvc;

namespace CRM.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CursoController : ControllerBase
    {
        private readonly ICursoService _cursoService;

        public CursoController(ICursoService cursoService)
        {
            _cursoService = cursoService;
        }

        [HttpPost("cadastrar")]
        public async Task<ActionResult<ResponseModel<CursoRetornoDto>>> Cadastrar([FromBody] CursoCadastroDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.NmCurso))
                return BadRequest(ResponseModel<CursoRetornoDto>.Erro("Nome do curso é obrigatório."));

            var curso = await _cursoService.CadastrarCursoAsync(dto);
            return Ok(ResponseModel<CursoRetornoDto>.Sucesso(curso, "Curso cadastrado com sucesso."));
        }

        [HttpPut("editar")]
        public async Task<ActionResult<ResponseModel<CursoRetornoDto>>> Editar([FromBody] CursoEdicaoDto dto)
        {
            var curso = await _cursoService.EditarCursoAsync(dto);
            if (curso == null)
                return NotFound(ResponseModel<CursoRetornoDto>.Erro("Curso não encontrado."));

            return Ok(ResponseModel<CursoRetornoDto>.Sucesso(curso, "Curso editado com sucesso."));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseModel<CursoRetornoDto>>> BuscarPorId(int id)
        {
            var curso = await _cursoService.BuscarPorIdAsync(id);
            if (curso == null)
                return NotFound(ResponseModel<CursoRetornoDto>.Erro("Curso não encontrado."));

            return Ok(ResponseModel<CursoRetornoDto>.Sucesso(curso));
        }

        [HttpGet("listar")]
        public async Task<ActionResult<ResponseModel<List<CursoRetornoDto>>>> ListarTodos()
        {
            var lista = await _cursoService.ListarTodosAsync();
            return Ok(ResponseModel<List<CursoRetornoDto>>.Sucesso(lista));
        }
    }
}
