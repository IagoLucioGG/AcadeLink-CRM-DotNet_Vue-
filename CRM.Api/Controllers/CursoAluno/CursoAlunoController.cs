using CRM.DTOs.CursoAluno_;
using CRM.Models.ResponseModel;
using CRM.Services.CursoAluno_;
using Microsoft.AspNetCore.Mvc;

namespace CRM.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CursoAlunoController : ControllerBase
    {
        private readonly ICursoAlunoService _cursoAlunoService;

        public CursoAlunoController(ICursoAlunoService cursoAlunoService)
        {
            _cursoAlunoService = cursoAlunoService;
        }

        [HttpPost("matricular")]
        public async Task<ActionResult<ResponseModel<CursoAlunoRetornoDto>>> Matricular([FromBody] CursoAlunoCadastroDto dto)
        {
            var matricula = await _cursoAlunoService.MatricularAsync(dto);
            return Ok(ResponseModel<CursoAlunoRetornoDto>.Sucesso(matricula, "Aluno matriculado com sucesso."));
        }

        [HttpPut("editar")]
        public async Task<ActionResult<ResponseModel<CursoAlunoRetornoDto>>> Editar([FromBody] CursoAlunoEdicaoDto dto)
        {
            var matricula = await _cursoAlunoService.EditarMatriculaAsync(dto);
            if (matricula == null)
                return NotFound(ResponseModel<CursoAlunoRetornoDto>.Erro("Matrícula não encontrada."));

            return Ok(ResponseModel<CursoAlunoRetornoDto>.Sucesso(matricula, "Matrícula editada com sucesso."));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseModel<CursoAlunoRetornoDto>>> BuscarPorId(int id)
        {
            var matricula = await _cursoAlunoService.BuscarPorIdAsync(id);
            if (matricula == null)
                return NotFound(ResponseModel<CursoAlunoRetornoDto>.Erro("Matrícula não encontrada."));

            return Ok(ResponseModel<CursoAlunoRetornoDto>.Sucesso(matricula));
        }

        [HttpGet("listar")]
        public async Task<ActionResult<ResponseModel<List<CursoAlunoRetornoDto>>>> ListarTodos()
        {
            var lista = await _cursoAlunoService.ListarTodosAsync();
            return Ok(ResponseModel<List<CursoAlunoRetornoDto>>.Sucesso(lista));
        }

        [HttpPatch("{id}/cancelar")]
        public async Task<ActionResult<ResponseModel<bool>>> CancelarMatricula(int id)
        {
            var sucesso = await _cursoAlunoService.CancelarMatriculaAsync(id);
            if (!sucesso)
                return NotFound(ResponseModel<bool>.Erro("Matrícula não encontrada."));

            return Ok(ResponseModel<bool>.Sucesso(true, "Matrícula cancelada com sucesso."));
        }
    }
}
