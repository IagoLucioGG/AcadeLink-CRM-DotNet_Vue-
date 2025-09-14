using CRM.Data;
using CRM.Maps;
using CRM.Models.Polo_;
using CRM.DTOs.Polo_;
using Microsoft.EntityFrameworkCore;
using CRM.Services.Util_;

namespace CRM.Services.Polo_
{
    public class PoloService : IPoloService
    {
        private readonly AppDbContext _context;
        private readonly Util _util;

        public PoloService(AppDbContext context, Util util)
        {
            _context = context;
            _util = util;
        }

        public async Task<PoloRetornoDto> CadastrarPoloAsync(PoloCadastroDto dto)
        {
            int idEmpresa = await _util.GetIdEmpresaFromToken();

            var polo = MapeadorModels.Montar<Polo, PoloCadastroDto>(dto);
            polo.IdEmpresa = idEmpresa;

            _context.Polos.Add(polo);
            await _context.SaveChangesAsync();

            return MapeadorModels.Montar<PoloRetornoDto, Polo>(polo);
        }

        public async Task<PoloRetornoDto?> EditarPoloAsync(PoloEdicaoDto dto)
        {
            int idEmpresa = await _util.GetIdEmpresaFromToken();

            var poloDb = await _context.Polos
                .FirstOrDefaultAsync(p => p.IdPolo == dto.IdPolo && p.IdEmpresa == idEmpresa);

            if (poloDb == null)
                return null;

            MapeadorModels.CopiarPropriedades(dto, poloDb);
            await _context.SaveChangesAsync();

            return MapeadorModels.Montar<PoloRetornoDto, Polo>(poloDb);
        }

        public async Task<PoloRetornoDto?> BuscarPorIdAsync(int idPolo)
        {
            int idEmpresa = await _util.GetIdEmpresaFromToken();

            var polo = await _context.Polos
                .FirstOrDefaultAsync(p => p.IdPolo == idPolo && p.IdEmpresa == idEmpresa);

            if (polo == null)
                return null;

            return MapeadorModels.Montar<PoloRetornoDto, Polo>(polo);
        }

        public async Task<List<PoloRetornoDto>> ListarTodosAsync()
        {
            int idEmpresa = await _util.GetIdEmpresaFromToken();

            return await _context.Polos
                .Where(p => p.IdEmpresa == idEmpresa)
                .Select(p => MapeadorModels.Montar<PoloRetornoDto, Polo>(p))
                .ToListAsync();
        }

        public async Task<bool> ExcluirPoloAsync(int idPolo)
        {
            int idEmpresa = await _util.GetIdEmpresaFromToken();

            var polo = await _context.Polos
                .FirstOrDefaultAsync(p => p.IdPolo == idPolo && p.IdEmpresa == idEmpresa);

            if (polo == null)
                return false;

            _context.Polos.Remove(polo);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
