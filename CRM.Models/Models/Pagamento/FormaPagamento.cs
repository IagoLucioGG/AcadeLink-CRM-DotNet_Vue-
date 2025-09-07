using System.ComponentModel.DataAnnotations;

namespace CRM.Models.Pagamento_
{
    public class FormaPagamento
    {
        [Key]
        public int IdFormaPagamento { get; set; }
        public string NmFormaPagamento { get; set; } = string.Empty;
        public bool GeraValor { get; set; }
        public int IdEmpresa { get; set; }
    }
}