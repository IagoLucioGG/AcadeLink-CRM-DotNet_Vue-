namespace CRM.DTOs.FormaPagamento_
{
    public class FormaPagamentoRequestDTO
    {
        public string NmFormaPagamento { get; set; } = string.Empty;
        public bool GeraValor { get; set; }
    }
}