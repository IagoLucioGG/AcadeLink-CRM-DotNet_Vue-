using System.ComponentModel.DataAnnotations;

namespace CRM.Models.CursoAluno_
{
    public class CursoAluno
    {
        [Key]
        public int IdCursoAluno { get; set; }
        public int IdAluno { get; set; }
        public int IdCurso { get; set; }
        public int NrMatricula { get; set; }
        public int IdEmpresa { get; set; }
        public int IdPolo { get; set; }
        public int IdModalidade { get; set; }
        public DateTime DataMatricula { get; set; }
        public DateTime? DataCancelamento { get; set; }
    }
}