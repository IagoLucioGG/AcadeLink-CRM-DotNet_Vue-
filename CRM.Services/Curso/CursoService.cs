using CRM.Data;
using CRM.Maps;
using CRM.Models.Curso;
using CRM.DTOs.Curso_;
using Microsoft.EntityFrameworkCore;
using CRM.Services.Util_;

namespace CRM.Services.Curso_
{
    public class CursoService : ICursoService
    {
        private readonly AppDbContext _context;
        private readonly Util _util;

        public CursoService(AppDbContext context, Util util)
        {
            _context = context;
            _util = util;
        }

        public async Task<CursoRetornoDto> CadastrarCursoAsync(CursoCadastroDto dto)
        {
            int idEmpresa = await _util.GetIdEmpresaFromToken();

            var curso = MapeadorModels.Montar<Curso, CursoCadastroDto>(dto);
            curso.IdEmpresa = idEmpresa;

            _context.Cursos.Add(curso);
            await _context.SaveChangesAsync();

            return MapeadorModels.Montar<CursoRetornoDto, Curso>(curso);
        }

        public async Task<CursoRetornoDto?> EditarCursoAsync(CursoEdicaoDto dto)
        {
            int idEmpresa = await _util.GetIdEmpresaFromToken();

            var cursoDb = await _context.Cursos
                .FirstOrDefaultAsync(c => c.IdCurso == dto.IdCurso && c.IdEmpresa == idEmpresa);

            if (cursoDb == null)
                return null;

            MapeadorModels.CopiarPropriedades(dto, cursoDb);
            await _context.SaveChangesAsync();

            return MapeadorModels.Montar<CursoRetornoDto, Curso>(cursoDb);
        }

        public async Task<CursoRetornoDto?> BuscarPorIdAsync(int idCurso)
        {
            int idEmpresa = await _util.GetIdEmpresaFromToken();

            var curso = await _context.Cursos
                .FirstOrDefaultAsync(c => c.IdCurso == idCurso && c.IdEmpresa == idEmpresa);

            if (curso == null)
                return null;

            return MapeadorModels.Montar<CursoRetornoDto, Curso>(curso);
        }

        public async Task<List<CursoRetornoDto>> ListarTodosAsync()
        {
            int idEmpresa = await _util.GetIdEmpresaFromToken();

            return await _context.Cursos
                .Where(c => c.IdEmpresa == idEmpresa)
                .Select(c => MapeadorModels.Montar<CursoRetornoDto, Curso>(c))
                .ToListAsync();
        }

        public async Task<bool> ExcluirCursoAsync(int idCurso)
        {
            int idEmpresa = await _util.GetIdEmpresaFromToken();

            var curso = await _context.Cursos
                .FirstOrDefaultAsync(c => c.IdCurso == idCurso && c.IdEmpresa == idEmpresa);

            if (curso == null)
                return false;

            _context.Cursos.Remove(curso);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
