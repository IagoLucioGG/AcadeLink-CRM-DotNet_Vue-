using CRM.DTOs.Curso_;

namespace CRM.Services.Curso_
{
    public interface ICursoService
    {
        Task<CursoRetornoDto> CadastrarCursoAsync(CursoCadastroDto dto);
        Task<CursoRetornoDto?> EditarCursoAsync(CursoEdicaoDto dto);
        Task<CursoRetornoDto?> BuscarPorIdAsync(int idCurso);
        Task<List<CursoRetornoDto>> ListarTodosAsync();
        Task<bool> ExcluirCursoAsync(int idCurso);
    }
}
