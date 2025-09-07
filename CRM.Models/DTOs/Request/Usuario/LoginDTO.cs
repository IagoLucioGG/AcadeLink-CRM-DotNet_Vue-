using CRM.Models.Empresa_;

namespace CRM.DTOs.Usuario_
{
    public class LoginRequestDto
    {
        public string Login { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
    }

    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public UsuarioRetornoDto Usuario { get; set; }
        public Empresa? Empresa { get; set; }
    }

    public class AlterarSenhaDto
    {
        public string NmLogin { get; set; } = string.Empty;
        public string SenhaAntiga { get; set; } = string.Empty;
        public string NovaSenha { get; set; } = string.Empty;
    }
}
