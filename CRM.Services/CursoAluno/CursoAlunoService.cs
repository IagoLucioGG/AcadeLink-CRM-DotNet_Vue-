using CRM.Data;
using CRM.Maps;
using CRM.Models.CursoAluno_;
using CRM.DTOs.CursoAluno_;
using Microsoft.EntityFrameworkCore;
using CRM.Services.Util_;

namespace CRM.Services.CursoAluno_
{
    public class CursoAlunoService : ICursoAlunoService
    {
        private readonly AppDbContext _context;
        private readonly Util _util;

        public CursoAlunoService(AppDbContext context, Util util)
        {
            _context = context;
            _util = util;
        }

        public async Task<CursoAlunoRetornoDto> MatricularAsync(CursoAlunoCadastroDto dto)
        {
            int idEmpresa = await _util.GetIdEmpresaFromToken();

            var cursoAluno = MapeadorModels.Montar<CursoAluno, CursoAlunoCadastroDto>(dto);
            cursoAluno.IdEmpresa = idEmpresa; // garante que a matrícula é vinculada à empresa do token
            cursoAluno.DataMatricula = DateTime.Now;

            _context.CursoAlunos.Add(cursoAluno);
            await _context.SaveChangesAsync();

            return MapeadorModels.Montar<CursoAlunoRetornoDto, CursoAluno>(cursoAluno);
        }

        public async Task<CursoAlunoRetornoDto?> EditarMatriculaAsync(CursoAlunoEdicaoDto dto)
        {
            int idEmpresa = await _util.GetIdEmpresaFromToken();

            var cursoAlunoDb = await _context.CursoAlunos
                .FirstOrDefaultAsync(ca => ca.IdCursoAluno == dto.IdCursoAluno && ca.IdEmpresa == idEmpresa);

            if (cursoAlunoDb == null)
                return null;

            MapeadorModels.CopiarPropriedades(dto, cursoAlunoDb);
            await _context.SaveChangesAsync();

            return MapeadorModels.Montar<CursoAlunoRetornoDto, CursoAluno>(cursoAlunoDb);
        }

        public async Task<CursoAlunoRetornoDto?> BuscarPorIdAsync(int idCursoAluno)
        {
            int idEmpresa = await _util.GetIdEmpresaFromToken();

            var cursoAluno = await _context.CursoAlunos
                .FirstOrDefaultAsync(ca => ca.IdCursoAluno == idCursoAluno && ca.IdEmpresa == idEmpresa);

            if (cursoAluno == null)
                return null;

            return MapeadorModels.Montar<CursoAlunoRetornoDto, CursoAluno>(cursoAluno);
        }

        public async Task<List<CursoAlunoRetornoDto>> ListarTodosAsync()
        {
            int idEmpresa = await _util.GetIdEmpresaFromToken();

            return await _context.CursoAlunos
                .Where(ca => ca.IdEmpresa == idEmpresa)
                .Select(ca => MapeadorModels.Montar<CursoAlunoRetornoDto, CursoAluno>(ca))
                .ToListAsync();
        }

        public async Task<bool> CancelarMatriculaAsync(int idCursoAluno)
        {
            int idEmpresa = await _util.GetIdEmpresaFromToken();

            var cursoAluno = await _context.CursoAlunos
                .FirstOrDefaultAsync(ca => ca.IdCursoAluno == idCursoAluno && ca.IdEmpresa == idEmpresa);

            if (cursoAluno == null)
                return false;

            cursoAluno.DataCancelamento = DateTime.Now;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
