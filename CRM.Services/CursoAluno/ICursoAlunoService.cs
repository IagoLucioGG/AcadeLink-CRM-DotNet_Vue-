using CRM.DTOs.CursoAluno_;

namespace CRM.Services.CursoAluno_
{
    public interface ICursoAlunoService
    {
        Task<CursoAlunoRetornoDto> MatricularAsync(CursoAlunoCadastroDto dto);
        Task<CursoAlunoRetornoDto?> EditarMatriculaAsync(CursoAlunoEdicaoDto dto);
        Task<CursoAlunoRetornoDto?> BuscarPorIdAsync(int idCursoAluno);
        Task<List<CursoAlunoRetornoDto>> ListarTodosAsync();
        Task<bool> CancelarMatriculaAsync(int idCursoAluno);
    }
}
