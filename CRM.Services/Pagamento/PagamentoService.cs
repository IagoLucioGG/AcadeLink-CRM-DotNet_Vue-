using CRM.Data;
using CRM.Models.Pagamento_;
using CRM.DTOs.Pagamento_;
using Microsoft.EntityFrameworkCore;
using CRM.Services.Util_;
using CRM.Maps;

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
            pagamento.IdEmpresa = idEmpresa;
            pagamento.DataInclusao = DateTime.Now;
            pagamento.ValorRestante = pagamento.ValorDevido - pagamento.ValorPago;

            _context.Pagamentos.Add(pagamento);
            await _context.SaveChangesAsync();

            return await BuscarPorIdAsync(pagamento.IdPagamento);
        }


        public async Task<PagamentoRetornoDto?> EditarAsync(PagamentoEdicaoDto dto)
        {
            int idEmpresa = await _util.GetIdEmpresaFromToken();

            var pagamentoDb = await _context.Pagamentos
                .FirstOrDefaultAsync(p => p.IdPagamento == dto.IdPagamento && p.IdEmpresa == idEmpresa);

            if (pagamentoDb == null)
                return null;

            MapeadorModels.CopiarPropriedades(dto, pagamentoDb);
            pagamentoDb.ValorRestante = dto.ValorDevido - dto.ValorPago;
            await _context.SaveChangesAsync();

            return await BuscarPorIdAsync(pagamentoDb.IdPagamento);
        }

        public async Task<PagamentoRetornoDto?> BuscarPorIdAsync(int id)
        {
            int idEmpresa = await _util.GetIdEmpresaFromToken();

            var pagamento = await _context.Pagamentos
                .Where(p => p.IdPagamento == id && p.IdEmpresa == idEmpresa)
                .Select(p => new PagamentoRetornoDto
                {
                    IdPagamento = p.IdPagamento,
                    DataInclusao = p.DataInclusao,
                    DataReferentePagamento = p.DataReferentePagamento,

                    Aluno = _context.Alunos
                        .Where(a => a.IdAluno == p.IdAluno)
                        .Select(a => new AlunoDto
                        {
                            IdAluno = a.IdAluno,
                            NmAluno = a.NmAluno,
                            Email = a.Email,
                            Telefone = a.Telefone
                        })
                        .FirstOrDefault(),

                    Matricula = _context.CursoAlunos
                        .Where(m => m.IdCursoAluno == p.IdMatricula)
                        .Select(m => new MatriculaDto
                        {
                            IdMatricula = m.IdCursoAluno,
                            NrMatricula = m.NrMatricula,
                            DataMatricula = m.DataMatricula
                        })
                        .FirstOrDefault(),

                    FormaPagamento = _context.FormaPagamentos
                        .Where(f => f.IdFormaPagamento == p.IdFormaPagamento)
                        .Select(f => new FormaPagamentoDto
                        {
                            IdFormaPagamento = f.IdFormaPagamento,
                            Nome = f.NmFormaPagamento
                        })
                        .FirstOrDefault(),

                    ValorDevido = p.ValorDevido,
                    ValorPago = p.ValorPago,
                    ValorRestante = p.ValorRestante
                })
                .FirstOrDefaultAsync();

            return pagamento;
        }

        public async Task<List<PagamentoRetornoDto>> ListarTodosAsync()
        {
            int idEmpresa = await _util.GetIdEmpresaFromToken();

            var pagamentos = await _context.Pagamentos
                .Where(p => p.IdEmpresa == idEmpresa)
                .Select(p => new PagamentoRetornoDto
                {
                    IdPagamento = p.IdPagamento,
                    DataInclusao = p.DataInclusao,
                    DataReferentePagamento = p.DataReferentePagamento,

                    Aluno = _context.Alunos
                        .Where(a => a.IdAluno == p.IdAluno)
                        .Select(a => new AlunoDto
                        {
                            IdAluno = a.IdAluno,
                            NmAluno = a.NmAluno,
                            Email = a.Email,
                            Telefone = a.Telefone
                        })
                        .FirstOrDefault(),

                    Matricula = _context.CursoAlunos
                        .Where(m => m.IdCursoAluno == p.IdMatricula)
                        .Select(m => new MatriculaDto
                        {
                            IdMatricula = m.IdCursoAluno,
                            NrMatricula = m.NrMatricula,
                            DataMatricula = m.DataMatricula
                        })
                        .FirstOrDefault(),

                    FormaPagamento = _context.FormaPagamentos
                        .Where(f => f.IdFormaPagamento == p.IdFormaPagamento)
                        .Select(f => new FormaPagamentoDto
                        {
                            IdFormaPagamento = f.IdFormaPagamento,
                            Nome = f.NmFormaPagamento
                        })
                        .FirstOrDefault(),

                    ValorDevido = p.ValorDevido,
                    ValorPago = p.ValorPago,
                    ValorRestante = p.ValorRestante
                })
                .ToListAsync();

            return pagamentos;
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
