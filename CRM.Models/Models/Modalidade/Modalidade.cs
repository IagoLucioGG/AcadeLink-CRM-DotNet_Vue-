using System.ComponentModel.DataAnnotations;

namespace CRM.Models.Modalidade_
{
    public class Modalidade
    {
        [Key]
        public int IdModalidade { get; set; }
        public string NmModalidade { get; set; } = string.Empty;
        public int IdEmpresa { get; set; }
    }
}