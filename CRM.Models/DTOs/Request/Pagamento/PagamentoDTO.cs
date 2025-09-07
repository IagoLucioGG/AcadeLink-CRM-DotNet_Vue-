namespace CRM.DTOs.Pagamento_
{
    public class PagamentoCadastroDto
    {
        public DateTime DataReferentePagamento { get; set; }
        public int IdFormaPagamento { get; set; }
        public int IdAluno { get; set; }
        public decimal ValorDevido { get; set; }
        public decimal ValorPago { get; set; }
        public decimal ValorRestante { get; set; }
    }

    public class PagamentoEdicaoDto
    {
        public int IdPagamento { get; set; }
        public DateTime DataReferentePagamento { get; set; }
        public int IdFormaPagamento { get; set; }
        public int IdAluno { get; set; }
        public decimal ValorDevido { get; set; }
        public decimal ValorPago { get; set; }
        public decimal ValorRestante { get; set; }
    }

    public class PagamentoRetornoDto
    {
        public int IdPagamento { get; set; }
        public DateTime DataInclusao { get; set; }
        public DateTime DataReferentePagamento { get; set; }
        public int IdFormaPagamento { get; set; }
        public int IdAluno { get; set; }
        public decimal ValorDevido { get; set; }
        public decimal ValorPago { get; set; }
        public decimal ValorRestante { get; set; }
    }
}
