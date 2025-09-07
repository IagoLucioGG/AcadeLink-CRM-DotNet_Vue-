using System.ComponentModel.DataAnnotations;

namespace CRM.Models.Polo_
{
    public class Polo
    {
        [Key]
        public int IdPolo { get; set; }
        public string NmPolo { get; set; } = string.Empty;
        public int IdEmpresa { get; set; }
    }
}