using CRM.Data;
using CRM.DTOs.CursoAluno_;
using CRM.Models.CursoAluno_;
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

            // valida existência de aluno e curso
            var aluno = await _context.Alunos
                .Where(a => a.IdAluno == dto.IdAluno && a.IdEmpresa == idEmpresa)
                .Select(a => new AlunoDto
                {
                    IdAluno = a.IdAluno,
                    NmAluno = a.NmAluno,
                    Email = a.Email,
                    Telefone = a.Telefone
                }).FirstOrDefaultAsync();
            if (aluno == null) throw new Exception("Aluno não encontrado.");

            var curso = await _context.Cursos
                .Where(c => c.IdCurso == dto.IdCurso && c.IdEmpresa == idEmpresa)
                .Select(c => new CursoDto
                {
                    IdCurso = c.IdCurso,
                    NmCurso = c.NmCurso
                }).FirstOrDefaultAsync();
            if (curso == null) throw new Exception("Curso não encontrado.");

            var empresa = await _context.Empresas
                .Where(e => e.IdEmpresa == idEmpresa)
                .Select(e => new EmpresaDto
                {
                    IdEmpresa = e.IdEmpresa,
                    Nome = e.NmEmpresa
                }).FirstOrDefaultAsync();

            var modalidade = await _context.Modalidades
                .Where(m => m.IdModalidade == dto.IdModalidade)
                .Select(m => new ModalidadeDto
                {
                    IdModalidade = m.IdModalidade,
                    NmModalidade = m.NmModalidade
                }).FirstOrDefaultAsync();

            var polo = await _context.Polos
                .Where(p => p.IdPolo == dto.IdPolo)
                .Select(p => new PoloDto
                {
                    IdPolo = p.IdPolo,
                    NmPolo = p.NmPolo
                }).FirstOrDefaultAsync();

            var cursoAluno = new CursoAluno
            {
                IdAluno = dto.IdAluno,
                IdCurso = dto.IdCurso,
                NrMatricula = dto.NrMatricula,
                IdEmpresa = idEmpresa,
                IdPolo = dto.IdPolo,
                IdModalidade = dto.IdModalidade,
                DataMatricula = DateTime.Now
            };

            _context.CursoAlunos.Add(cursoAluno);
            await _context.SaveChangesAsync();

            return new CursoAlunoRetornoDto
            {
                IdCursoAluno = cursoAluno.IdCursoAluno,
                Aluno = aluno,
                Curso = curso,
                Empresa = empresa,
                Modalidade = modalidade,
                Polo = polo,
                NrMatricula = cursoAluno.NrMatricula,
                DataMatricula = cursoAluno.DataMatricula,
                DataCancelamento = cursoAluno.DataCancelamento
            };
        }

        public async Task<CursoAlunoRetornoDto?> EditarMatriculaAsync(CursoAlunoEdicaoDto dto)
        {
            int idEmpresa = await _util.GetIdEmpresaFromToken();

            var cursoAlunoDb = await _context.CursoAlunos
                .FirstOrDefaultAsync(ca => ca.IdCursoAluno == dto.IdCursoAluno && ca.IdEmpresa == idEmpresa);
            if (cursoAlunoDb == null) return null;

            // atualizar campos
            cursoAlunoDb.IdAluno = dto.IdAluno;
            cursoAlunoDb.IdCurso = dto.IdCurso;
            cursoAlunoDb.NrMatricula = dto.NrMatricula;
            cursoAlunoDb.IdPolo = dto.IdPolo;
            cursoAlunoDb.IdModalidade = dto.IdModalidade;
            cursoAlunoDb.DataMatricula = dto.DataMatricula;
            cursoAlunoDb.DataCancelamento = dto.DataCancelamento;

            await _context.SaveChangesAsync();

            return await MontarRetorno(cursoAlunoDb);
        }

        public async Task<CursoAlunoRetornoDto?> BuscarPorIdAsync(int idCursoAluno)
        {
            int idEmpresa = await _util.GetIdEmpresaFromToken();

            var cursoAluno = await _context.CursoAlunos
                .FirstOrDefaultAsync(ca => ca.IdCursoAluno == idCursoAluno && ca.IdEmpresa == idEmpresa);
            if (cursoAluno == null) return null;

            return await MontarRetorno(cursoAluno);
        }

        public async Task<List<CursoAlunoRetornoDto>> ListarTodosAsync()
        {
            int idEmpresa = await _util.GetIdEmpresaFromToken();

            var cursoAlunos = await _context.CursoAlunos
                .Where(ca => ca.IdEmpresa == idEmpresa)
                .ToListAsync();

            var resultados = new List<CursoAlunoRetornoDto>();
            foreach (var ca in cursoAlunos)
            {
                resultados.Add(await MontarRetorno(ca));
            }

            return resultados;
        }

        public async Task<bool> CancelarMatriculaAsync(int idCursoAluno)
        {
            int idEmpresa = await _util.GetIdEmpresaFromToken();

            var cursoAluno = await _context.CursoAlunos
                .FirstOrDefaultAsync(ca => ca.IdCursoAluno == idCursoAluno && ca.IdEmpresa == idEmpresa);
            if (cursoAluno == null) return false;

            cursoAluno.DataCancelamento = DateTime.Now;
            await _context.SaveChangesAsync();

            return true;
        }

        // método privado para montar retorno completo
        private async Task<CursoAlunoRetornoDto> MontarRetorno(CursoAluno ca)
        {
            int idEmpresa = ca.IdEmpresa;

            var aluno = await _context.Alunos
                .Where(a => a.IdAluno == ca.IdAluno && a.IdEmpresa == idEmpresa)
                .Select(a => new AlunoDto
                {
                    IdAluno = a.IdAluno,
                    NmAluno = a.NmAluno,
                    Email = a.Email,
                    Telefone = a.Telefone
                }).FirstOrDefaultAsync();

            var curso = await _context.Cursos
                .Where(c => c.IdCurso == ca.IdCurso && c.IdEmpresa == idEmpresa)
                .Select(c => new CursoDto
                {
                    IdCurso = c.IdCurso,
                    NmCurso = c.NmCurso
                }).FirstOrDefaultAsync();

            var empresa = await _context.Empresas
                .Where(e => e.IdEmpresa == idEmpresa)
                .Select(e => new EmpresaDto
                {
                    IdEmpresa = e.IdEmpresa,
                    Nome = e.NmEmpresa
                }).FirstOrDefaultAsync();

            var modalidade = await _context.Modalidades
                .Where(m => m.IdModalidade == ca.IdModalidade)
                .Select(m => new ModalidadeDto
                {
                    IdModalidade = m.IdModalidade,
                    NmModalidade = m.NmModalidade
                }).FirstOrDefaultAsync();

            var polo = await _context.Polos
                .Where(p => p.IdPolo == ca.IdPolo)
                .Select(p => new PoloDto
                {
                    IdPolo = p.IdPolo,
                    NmPolo = p.NmPolo
                }).FirstOrDefaultAsync();

            return new CursoAlunoRetornoDto
            {
                IdCursoAluno = ca.IdCursoAluno,
                Aluno = aluno,
                Curso = curso,
                Empresa = empresa,
                Modalidade = modalidade,
                Polo = polo,
                NrMatricula = ca.NrMatricula,
                DataMatricula = ca.DataMatricula,
                DataCancelamento = ca.DataCancelamento
            };
        }
    }
}
