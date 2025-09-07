namespace CRM.DTOs.CursoAluno_
{
    public class CursoAlunoCadastroDto
    {
        public int IdAluno { get; set; }
        public int IdCurso { get; set; }
        public int IdModalidade { get; set; }
    }

    public class CursoAlunoEdicaoDto
    {
        public int IdCursoAluno { get; set; }
        public int IdAluno { get; set; }
        public int IdCurso { get; set; }
        public int IdModalidade { get; set; }
        public DateTime DataMatricula { get; set; }
        public DateTime DataCancelamento { get; set; }
    }

    public class CursoAlunoRetornoDto
    {
        public int IdCursoAluno { get; set; }
        public int IdAluno { get; set; }
        public int IdCurso { get; set; }
        public int IdModalidade { get; set; }
        public DateTime DataMatricula { get; set; }
        public DateTime DataCancelamento { get; set; }
    }
}
