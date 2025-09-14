namespace CRM.DTOs.CursoAluno_
{
    public class CursoAlunoCadastroDto
    {
        public int IdAluno { get; set; }
        public int IdCurso { get; set; }
        public int NrMatricula { get; set; }
        public int IdPolo { get; set; }
        public int IdModalidade { get; set; }
        public DateTime DataMatricula { get; set; }
    }

    public class CursoAlunoEdicaoDto
    {
        public int IdCursoAluno { get; set; }
        public int IdAluno { get; set; }
        public int IdCurso { get; set; }
        public int NrMatricula { get; set; }
        public int IdPolo { get; set; }
        public int IdModalidade { get; set; }
        public DateTime DataMatricula { get; set; }
        public DateTime? DataCancelamento { get; set; }
    }

    public class AlunoDto
    {
        public int IdAluno { get; set; }
        public string NmAluno { get; set; }
        public string Email { get; set; }
        public long Telefone { get; set; }
    }

    public class CursoDto
    {
        public int IdCurso { get; set; }
        public string NmCurso { get; set; }
    }

    public class EmpresaDto
    {
        public int IdEmpresa { get; set; }
        public string Nome { get; set; }
    }

    public class ModalidadeDto
    {
        public int IdModalidade { get; set; }
        public string NmModalidade { get; set; }
    }

    public class PoloDto
    {
        public int IdPolo { get; set; }
        public string NmPolo { get; set; }
    }

    public class CursoAlunoRetornoDto
    {
        public int IdCursoAluno { get; set; }
        public AlunoDto Aluno { get; set; }
        public CursoDto Curso { get; set; }
        public EmpresaDto Empresa { get; set; }
        public PoloDto Polo { get; set; }
        public ModalidadeDto Modalidade { get; set; }
        public int NrMatricula { get; set; }
        public DateTime DataMatricula { get; set; }
        public DateTime? DataCancelamento { get; set; }
    }
}
