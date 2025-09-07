using CRM.Data;
using CRM.Maps;
using CRM.Models.Pagamento_;
using CRM.DTOs.Pagamento_;
using Microsoft.EntityFrameworkCore;
using CRM.Services.Util_;

namespace CRM.Services.Pagamento_
{
    public class PagamentoService : IPagamentoService
    {
        private readonly AppDbContext _context;
        private readonly Util _util;

        public PagamentoService(AppDbContext context, Util util)
        {
            _context = context;
            _util = util;
        }

        public async Task<PagamentoRetornoDto> CadastrarAsync(PagamentoCadastroDto dto)
        {
            int idEmpresa = await _util.GetIdEmpresaFromToken();

            var pagamento = MapeadorModels.Montar<Pagamento, PagamentoCadastroDto>(dto);
            pagamento.IdEmpresa = idEmpresa; // Vincula ao IdEmpresa do token
            pagamento.DataInclusao = DateTime.Now;

            _context.Pagamentos.Add(pagamento);
            await _context.SaveChangesAsync();

            return MapeadorModels.Montar<PagamentoRetornoDto, Pagamento>(pagamento);
        }

        public async Task<PagamentoRetornoDto?> EditarAsync(PagamentoEdicaoDto dto)
        {
            int idEmpresa = await _util.GetIdEmpresaFromToken();

            var pagamentoDb = await _context.Pagamentos
                .FirstOrDefaultAsync(p => p.IdPagamento == dto.IdPagamento && p.IdEmpresa == idEmpresa);

            if (pagamentoDb == null)
                return null;

            MapeadorModels.CopiarPropriedades(dto, pagamentoDb);
            await _context.SaveChangesAsync();

            return MapeadorModels.Montar<PagamentoRetornoDto, Pagamento>(pagamentoDb);
        }

        public async Task<PagamentoRetornoDto?> BuscarPorIdAsync(int id)
        {
            int idEmpresa = await _util.GetIdEmpresaFromToken();

            var pagamento = await _context.Pagamentos
                .FirstOrDefaultAsync(p => p.IdPagamento == id && p.IdEmpresa == idEmpresa);

            if (pagamento == null)
                return null;

            return MapeadorModels.Montar<PagamentoRetornoDto, Pagamento>(pagamento);
        }

        public async Task<List<PagamentoRetornoDto>> ListarTodosAsync()
        {
            int idEmpresa = await _util.GetIdEmpresaFromToken();

            return await _context.Pagamentos
                .Where(p => p.IdEmpresa == idEmpresa)
                .Select(p => MapeadorModels.Montar<PagamentoRetornoDto, Pagamento>(p))
                .ToListAsync();
        }

        public async Task<bool> ExcluirAsync(int id)
        {
            int idEmpresa = await _util.GetIdEmpresaFromToken();

            var pagamento = await _context.Pagamentos
                .FirstOrDefaultAsync(p => p.IdPagamento == id && p.IdEmpresa == idEmpresa);

            if (pagamento == null)
                return false;

            _context.Pagamentos.Remove(pagamento);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
