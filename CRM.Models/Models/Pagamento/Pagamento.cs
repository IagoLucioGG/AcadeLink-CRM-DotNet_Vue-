using System.ComponentModel.DataAnnotations;

namespace CRM.Models.Pagamento_
{
    public class Pagamento
    {
        [Key]
        public int IdPagamento { get; set; }
        public DateTime DataInclusao { get; set; }
        public DateTime DataReferentePagamento { get; set; }
        public int IdFormaPagamento { get; set; }
        public int IdAluno { get; set; }
        public int IdMatricula { get; set; }
        public decimal ValorDevido { get; set; }
        public decimal ValorPago { get; set; }
        public decimal ValorRestante { get; set; }
        public int IdEmpresa { get; set; }
    }
}