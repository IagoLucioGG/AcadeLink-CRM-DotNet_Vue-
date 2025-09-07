using CRM.DTOs.Aluno_;

namespace CRM.Services.Aluno_
{
    public interface IAlunoService
    {
        Task<AlunoRetornoDto> CadastrarAlunoAsync(AlunoCadastroDto dto);
        Task<AlunoRetornoDto?> EditarAlunoAsync(AlunoEdicaoDto dto);
        Task<AlunoRetornoDto?> BuscarPorIdAsync(int idAluno);
        Task<List<AlunoRetornoDto>> ListarTodosAsync();
        Task<bool> ExcluirAlunoAsync(int idAluno);
    }
}
