using CRM.DTOs.Polo_;

namespace CRM.Services.Polo_
{
    public interface IPoloService
    {
        Task<PoloRetornoDto> CadastrarPoloAsync(PoloCadastroDto dto);
        Task<PoloRetornoDto?> EditarPoloAsync(PoloEdicaoDto dto);
        Task<PoloRetornoDto?> BuscarPorIdAsync(int idPolo);
        Task<List<PoloRetornoDto>> ListarTodosAsync();
        Task<bool> ExcluirPoloAsync(int idPolo);
    }
}
