namespace CRM.DTOs.Aluno_
{
    public class AlunoCadastroDto
    {
        public string NmAluno { get; set; } = string.Empty;
        public long Telefone { get; set; }
        public string Email { get; set; } = string.Empty;
    }

    public class AlunoEdicaoDto
    {
        public int IdAluno { get; set; }
        public string NmAluno { get; set; } = string.Empty;
        public long Telefone { get; set; }
        public string Email { get; set; } = string.Empty;
    }

    public class AlunoRetornoDto
    {
        public int IdAluno { get; set; }
        public string NmAluno { get; set; } = string.Empty;
        public long Telefone { get; set; }
        public string Email { get; set; } = string.Empty;
    }
}
