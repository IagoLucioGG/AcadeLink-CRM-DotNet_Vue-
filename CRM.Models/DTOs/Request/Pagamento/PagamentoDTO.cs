namespace CRM.DTOs.Pagamento_
{
    public class PagamentoCadastroDto
    {
        public DateTime DataReferentePagamento { get; set; }
        public int IdFormaPagamento { get; set; }
        public int IdAluno { get; set; }
        public int IdMatricula { get; set; }
        public decimal ValorDevido { get; set; }
        public decimal ValorPago { get; set; }
        
    }

    public class PagamentoEdicaoDto
    {
        public int IdPagamento { get; set; }
        public DateTime DataReferentePagamento { get; set; }
        public int IdFormaPagamento { get; set; }
        public int IdMatricula { get; set; }
        public int IdAluno { get; set; }
        public decimal ValorDevido { get; set; }
        public decimal ValorPago { get; set; }
        public decimal ValorRestante { get; set; }
    }

    // ======================
    // Dtos relacionados
    // ======================
    public class AlunoDto
    {
        public int IdAluno { get; set; }
        public string NmAluno { get; set; }
        public string Email { get; set; }
        public long Telefone { get; set; }
    }

    public class MatriculaDto
    {
        public int IdMatricula { get; set; }
        public int NrMatricula { get; set; }
        public DateTime DataMatricula { get; set; }
    }

    public class FormaPagamentoDto
    {
        public int IdFormaPagamento { get; set; }
        public string Nome { get; set; }
    }

    // ======================
    // Retorno principal
    // ======================
    public class PagamentoRetornoDto
    {
        public int IdPagamento { get; set; }
        public DateTime DataInclusao { get; set; }
        public DateTime DataReferentePagamento { get; set; }

        public AlunoDto Aluno { get; set; }
        public MatriculaDto Matricula { get; set; }
        public FormaPagamentoDto FormaPagamento { get; set; }

        public decimal ValorDevido { get; set; }
        public decimal ValorPago { get; set; }
        public decimal ValorRestante { get; set; }
    }
}
