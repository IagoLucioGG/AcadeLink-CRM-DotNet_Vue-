namespace CRM.DTOs.Usuario_
{
    public class UsuarioCadastroDto
    {
        public string NmAgente { get; set; } = string.Empty;
        public string NmLogin { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public int IdEmpresa { get; set; }
    }

    public class UsuarioEdicaoDto
    {
        public int IdUsuario { get; set; }
        public string NmAgente { get; set; } = string.Empty;
        public string NmLogin { get; set; } = string.Empty;
        public string? Senha { get; set; }
    }

    public class UsuarioRetornoDto
{
    public int IdUsuario { get; set; }
    public string NmAgente { get; set; } = string.Empty;
    public string NmLogin { get; set; } = string.Empty;
    public bool Ativo { get; set; }
    public int IdEmpresa { get; set; } // ✅ Novo campo
}

}
