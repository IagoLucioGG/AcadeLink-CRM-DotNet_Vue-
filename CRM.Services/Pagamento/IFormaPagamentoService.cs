

using CRM.DTOs.FormaPagamento_;
using CRM.DTOs.Pagamento_;
using CRM.Models.Pagamento_;

namespace CRM.Services.Pagamento_
{
    public interface IFormaPagamentoService
    {
        Task<FormaPagamento> CadastrarAsync(FormaPagamentoRequestDTO dto);
        Task<FormaPagamento?> EditarAsync(FormaPagamento dto);
        Task<FormaPagamento?> BuscarPorIdAsync(int id);
        Task<List<FormaPagamento>> ListarTodosAsync();
        Task<bool> ExcluirAsync(int id);
    }
}
