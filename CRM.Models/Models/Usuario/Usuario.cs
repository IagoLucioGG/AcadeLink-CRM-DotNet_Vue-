using System.ComponentModel.DataAnnotations;

namespace CRM.Models.Usuario_
{
    public class Usuario
    {
        [Key]
        public int IdUsuario { get; set; }
        public string NmAgente { get; set; } = string.Empty;
        public string NmLogin { get; set; } = string.Empty;
        [Required]
        public string Senha { get; set; } = string.Empty;
        public bool Ativo { get; set; } = true;
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public int IdEmpresa { get; set; }
    }
}