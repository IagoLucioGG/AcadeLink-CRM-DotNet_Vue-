using CRM.Data;
using CRM.Maps;
using CRM.Models.Aluno_Cliente;
using CRM.DTOs.Aluno_;
using Microsoft.EntityFrameworkCore;
using CRM.Services.Util_;
using CRM.Exceptions;

namespace CRM.Services.Aluno_
{
    public class AlunoService : IAlunoService
    {
        private readonly AppDbContext _context;
        private readonly Util _util;

        public AlunoService(AppDbContext context, Util util)
        {
            _context = context;
            _util = util;
        }

        public async Task<AlunoRetornoDto> CadastrarAlunoAsync(AlunoCadastroDto dto)
        {
            
            int idEmpresa = await _util.GetIdEmpresaFromToken();
            var alunoExistente = await _context.Alunos.FirstOrDefaultAsync(a => a.CPF == dto.CPF && a.IdEmpresa == idEmpresa);
            if (alunoExistente != null)
                throw new ConflictException($"Já existe um Aluno com esse CPF: {alunoExistente.NmAluno}");
            var aluno = MapeadorModels.Montar<Aluno, AlunoCadastroDto>(dto);
            aluno.IdEmpresa = idEmpresa;

            _context.Alunos.Add(aluno);
            await _context.SaveChangesAsync();

            return MapeadorModels.Montar<AlunoRetornoDto, Aluno>(aluno);
        }

        public async Task<AlunoRetornoDto?> EditarAlunoAsync(AlunoEdicaoDto dto)
        {
            int idEmpresa = await _util.GetIdEmpresaFromToken();
            var alunoExistente = await _context.Alunos.FirstOrDefaultAsync(a => a.CPF == dto.CPF && a.IdAluno != dto.IdAluno &&  a.IdEmpresa == idEmpresa);
            if (alunoExistente != null)
                throw new ConflictException($"Já existe um Aluno com esse CPF: {alunoExistente.NmAluno}");
            
            var alunoDb = await _context.Alunos.FirstOrDefaultAsync(a=> a.IdAluno == dto.IdAluno && a.IdEmpresa == idEmpresa);
            if (alunoDb == null)
                return null;

            MapeadorModels.CopiarPropriedades(dto, alunoDb);
            await _context.SaveChangesAsync();

            return MapeadorModels.Montar<AlunoRetornoDto, Aluno>(alunoDb);
        }

        public async Task<AlunoRetornoDto?> BuscarPorIdAsync(int idAluno)
        {
            int idEmpresa = await _util.GetIdEmpresaFromToken();

            var aluno = await _context.Alunos
                .FirstOrDefaultAsync(a => a.IdAluno == idAluno && a.IdEmpresa == idEmpresa);

            if (aluno == null)
                return null;

            return MapeadorModels.Montar<AlunoRetornoDto, Aluno>(aluno);
        }


        public async Task<List<AlunoRetornoDto>> ListarTodosAsync()
        {
            int idEmpresa = await _util.GetIdEmpresaFromToken();
            return await _context.Alunos
                .Where(a => a.IdEmpresa == idEmpresa)
                .Select(a => MapeadorModels.Montar<AlunoRetornoDto, Aluno>(a))
                .ToListAsync();
        }

        public async Task<bool> ExcluirAlunoAsync(int idAluno)
        {
            int idEmpresa = await _util.GetIdEmpresaFromToken();
            var aluno = await _context.Alunos.FirstOrDefaultAsync(a => a.IdAluno == idAluno && a.IdEmpresa == idEmpresa);
            if (aluno == null)
                return false;

            _context.Alunos.Remove(aluno);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
