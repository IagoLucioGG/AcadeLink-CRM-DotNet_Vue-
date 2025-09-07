using System.ComponentModel.DataAnnotations;

namespace CRM.Models.Aluno_Cliente
{
    public class Aluno
    {
        [Key]
        public int IdAluno { get; set; }

        [Required(ErrorMessage = "O nome do aluno é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome do aluno não pode ter mais que 100 caracteres.")]
        public string NmAluno { get; set; } = string.Empty;


        [EmailAddress(ErrorMessage = "O e-mail informado não é válido.")]
        [StringLength(150, ErrorMessage = "O e-mail não pode ter mais que 150 caracteres.")]
        public string Email { get; set; } = string.Empty;
        public long Telefone { get; set; }
        public int IdEmpresa { get; set; }
    }
}
