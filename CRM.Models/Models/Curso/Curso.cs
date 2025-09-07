using System.ComponentModel.DataAnnotations;

namespace CRM.Models.Curso
{
    public class Curso
    {
        [Key]
        public int IdCurso { get; set; }
        public string NmCurso { get; set; } = string.Empty;
        public int IdEmpresa { get; set; }
    }
}