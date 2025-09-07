using CRM.DTOs.Pagamento_;

namespace CRM.Services.Pagamento_
{
    public interface IPagamentoService
    {
        Task<PagamentoRetornoDto> CadastrarAsync(PagamentoCadastroDto dto);
        Task<PagamentoRetornoDto?> EditarAsync(PagamentoEdicaoDto dto);
        Task<PagamentoRetornoDto?> BuscarPorIdAsync(int id);
        Task<List<PagamentoRetornoDto>> ListarTodosAsync();
        Task<bool> ExcluirAsync(int id);
    }
}
