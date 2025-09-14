using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CRM.Models.Aluno_Cliente;

namespace CRM.Models.Atendimento_
{
    public class Atendimento
    {
        [Key]
        public int IdAtendimento { get; set; }
        public int Cdchamada { get; set; }
        public int IdAluno { get; set; }
        [Required]
        public int IdUsuario { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string DsParecer { get; set; } = string.Empty;
        public DateTime DataAtendimento { get; set; }
        public bool TevePagamento { get; set; }
        public int IdPagamento { get; set; }
        public int IdEmpresa { get; set; }
    }
}