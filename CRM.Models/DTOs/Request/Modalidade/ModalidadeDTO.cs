namespace CRM.DTOs.Modalidade_
{
    public class ModalidadeCadastroDto
    {
        public string NmModalidade { get; set; } = string.Empty;
    }

    public class ModalidadeEdicaoDto
    {
        public int IdModalidade { get; set; }
        public string NmModalidade { get; set; } = string.Empty;
    }

    public class ModalidadeRetornoDto
    {
        public int IdModalidade { get; set; }
        public string NmModalidade { get; set; } = string.Empty;
    }
}
