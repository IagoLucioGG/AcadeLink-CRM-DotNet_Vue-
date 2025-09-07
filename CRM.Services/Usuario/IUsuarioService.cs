using CRM.DTOs.Usuario_;

namespace CRM.Services.Usuario_
{
    public interface IUsuarioService
    {
        Task<UsuarioRetornoDto> CadastrarUsuarioAsync(UsuarioCadastroDto usuario);
        Task<UsuarioRetornoDto?> LoginAsync(string login, string senha);
        Task<UsuarioRetornoDto?> EditarUsuarioAsync(UsuarioEdicaoDto usuario);
        Task<bool> AlterarStatusUsuarioAsync(int idUsuario, bool ativo);
        public string GerarToken(UsuarioRetornoDto usuario);
        Task<bool> AlterarSenhaAsync(string login, string senhaAntiga, string novaSenha);
        Task<IEnumerable<UsuarioRetornoDto>> ListarUsuariosAsync();
        Task<bool> SalvarSessao(int idUsuario, int idEmpresa, string token);
        Task<bool> LogoutAsync(int idUsuario);


    }

}