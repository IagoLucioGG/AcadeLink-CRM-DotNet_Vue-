namespace CRM.DTOs.Curso_
{
    public class CursoCadastroDto
    {
        public string NmCurso { get; set; } = string.Empty;
    }

    public class CursoEdicaoDto
    {
        public int IdCurso { get; set; }
        public string NmCurso { get; set; } = string.Empty;
    }

    public class CursoRetornoDto
    {
        public int IdCurso { get; set; }
        public string NmCurso { get; set; } = string.Empty;
    }
}
