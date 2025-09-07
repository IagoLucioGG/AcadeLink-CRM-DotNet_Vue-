using CRM.Data;
using CRM.Maps;
using CRM.Models.Modalidade_;
using CRM.DTOs.Modalidade_;
using Microsoft.EntityFrameworkCore;
using CRM.Services.Util_;

namespace CRM.Services.Modalidade_
{
    public class ModalidadeService : IModalidadeService
    {
        private readonly AppDbContext _context;
        private readonly Util _util;

        public ModalidadeService(AppDbContext context, Util util)
        {
            _context = context;
            _util = util;
        }

        public async Task<ModalidadeRetornoDto> CadastrarModalidadeAsync(ModalidadeCadastroDto dto)
        {
            int idEmpresa = await _util.GetIdEmpresaFromToken();

            var modalidade = MapeadorModels.Montar<Modalidade, ModalidadeCadastroDto>(dto);
            modalidade.IdEmpresa = idEmpresa;

            _context.Modalidades.Add(modalidade);
            await _context.SaveChangesAsync();

            return MapeadorModels.Montar<ModalidadeRetornoDto, Modalidade>(modalidade);
        }

        public async Task<ModalidadeRetornoDto?> EditarModalidadeAsync(ModalidadeEdicaoDto dto)
        {
            int idEmpresa = await _util.GetIdEmpresaFromToken();

            var modalidadeDb = await _context.Modalidades
                .FirstOrDefaultAsync(m => m.IdModalidade == dto.IdModalidade && m.IdEmpresa == idEmpresa);

            if (modalidadeDb == null)
                return null;

            MapeadorModels.CopiarPropriedades(dto, modalidadeDb);
            await _context.SaveChangesAsync();

            return MapeadorModels.Montar<ModalidadeRetornoDto, Modalidade>(modalidadeDb);
        }

        public async Task<ModalidadeRetornoDto?> BuscarPorIdAsync(int idModalidade)
        {
            int idEmpresa = await _util.GetIdEmpresaFromToken();

            var modalidade = await _context.Modalidades
                .FirstOrDefaultAsync(m => m.IdModalidade == idModalidade && m.IdEmpresa == idEmpresa);

            if (modalidade == null)
                return null;

            return MapeadorModels.Montar<ModalidadeRetornoDto, Modalidade>(modalidade);
        }

        public async Task<List<ModalidadeRetornoDto>> ListarTodasAsync()
        {
            int idEmpresa = await _util.GetIdEmpresaFromToken();

            return await _context.Modalidades
                .Where(m => m.IdEmpresa == idEmpresa)
                .Select(m => MapeadorModels.Montar<ModalidadeRetornoDto, Modalidade>(m))
                .ToListAsync();
        }

        public async Task<bool> ExcluirModalidadeAsync(int idModalidade)
        {
            int idEmpresa = await _util.GetIdEmpresaFromToken();

            var modalidade = await _context.Modalidades
                .FirstOrDefaultAsync(m => m.IdModalidade == idModalidade && m.IdEmpresa == idEmpresa);

            if (modalidade == null)
                return false;

            _context.Modalidades.Remove(modalidade);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
