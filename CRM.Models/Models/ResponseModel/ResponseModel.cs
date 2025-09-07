namespace CRM.Models.ResponseModel
{
    public class ResponseModel<T>
    {
        public string Mensagem { get; set; } = string.Empty;
        public bool Status { get; set; }
        public T? Dados { get; set; }

        public ResponseModel(bool status, string mensagem, T? dados)
        {
            Status = status;
            Mensagem = mensagem;
            Dados = status ? dados : default;
        }

        // ----------------------
        // Fábricas para facilitar
        // ----------------------

        // Para criar respostas de sucesso
        public static ResponseModel<T> Sucesso(T dados, string mensagem = "Operação realizada com sucesso")
        {
            return new ResponseModel<T>(true, mensagem, dados);
        }

        // Para criar respostas de erro
        public static ResponseModel<T> Erro(string mensagem)
        {
            return new ResponseModel<T>(false, mensagem, default);
        }
    }
}
