using System.ComponentModel.DataAnnotations;

namespace CRM.Models.Usuario_
{
    public class SessaoUsuario
{
    [Key]
    public Guid IdSessao { get; set; } = Guid.NewGuid();
     public int IdUsuario { get; set; }
    public int IdEmpresa { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTime DataCriacao { get; set; }
    public DateTime DataExpiracao { get; set; }
    public DateTime UltimaRequisicao { get; set; } 
}

}