using CRM.Data;
using CRM.DTOs.Pagamento_;
using CRM.Models.Pagamento_;
using CRM.Services.Util_;
using CRM.Maps;
using Microsoft.EntityFrameworkCore;
using CRM.DTOs.FormaPagamento_;

namespace CRM.Services.Pagamento_
{
    public class FormaPagamentoService : IFormaPagamentoService
    {
        private readonly AppDbContext _context;
        private readonly Util _util;

        public FormaPagamentoService(AppDbContext context, Util util)
        {
            _context = context;
            _util = util;
        }

        public async Task<FormaPagamento> CadastrarAsync(FormaPagamentoRequestDTO dto)
        {
            int idEmpresa = await _util.GetIdEmpresaFromToken();

            var formaPagamento = MapeadorModels.Montar<FormaPagamento, FormaPagamentoRequestDTO>(dto);
            formaPagamento.IdEmpresa = idEmpresa;

            _context.FormaPagamentos.Add(formaPagamento);
            await _context.SaveChangesAsync();

            return formaPagamento;
        }

        public async Task<FormaPagamento?> EditarAsync(FormaPagamento dto)
        {
            int idEmpresa = await _util.GetIdEmpresaFromToken();

            var formaPagamentoDb = await _context.FormaPagamentos
                .FirstOrDefaultAsync(f => f.IdFormaPagamento == dto.IdFormaPagamento && f.IdEmpresa == idEmpresa);

            if (formaPagamentoDb == null)
                return null;

            MapeadorModels.CopiarPropriedades(dto, formaPagamentoDb);
            await _context.SaveChangesAsync();

            return formaPagamentoDb;
        }

        public async Task<FormaPagamento?> BuscarPorIdAsync(int id)
        {
            int idEmpresa = await _util.GetIdEmpresaFromToken();

            return await _context.FormaPagamentos
                .FirstOrDefaultAsync(f => f.IdFormaPagamento == id && f.IdEmpresa == idEmpresa);
        }

        public async Task<List<FormaPagamento>> ListarTodosAsync()
        {
            int idEmpresa = await _util.GetIdEmpresaFromToken();

            return await _context.FormaPagamentos
                .Where(f => f.IdEmpresa == idEmpresa)
                .ToListAsync();
        }

        public async Task<bool> ExcluirAsync(int id)
        {
            int idEmpresa = await _util.GetIdEmpresaFromToken();

            var formaPagamento = await _context.FormaPagamentos
                .FirstOrDefaultAsync(f => f.IdFormaPagamento == id && f.IdEmpresa == idEmpresa);

            if (formaPagamento == null)
                return false;

            _context.FormaPagamentos.Remove(formaPagamento);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
