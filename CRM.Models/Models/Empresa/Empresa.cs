using System.ComponentModel.DataAnnotations;

namespace CRM.Models.Empresa_
{
    public class Empresa
    {
        [Key]
        public int IdEmpresa { get; set; }
        public string NmEmpresa { get; set; } = string.Empty;
        public bool Ativo { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public required string CNPJ { get; set; }
        public int QuantUsuarios { get; set; }
    }
}