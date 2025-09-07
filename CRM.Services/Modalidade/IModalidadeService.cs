using CRM.DTOs.Modalidade_;

namespace CRM.Services.Modalidade_
{
    public interface IModalidadeService
    {
        Task<ModalidadeRetornoDto> CadastrarModalidadeAsync(ModalidadeCadastroDto dto);
        Task<ModalidadeRetornoDto?> EditarModalidadeAsync(ModalidadeEdicaoDto dto);
        Task<ModalidadeRetornoDto?> BuscarPorIdAsync(int idModalidade);
        Task<List<ModalidadeRetornoDto>> ListarTodasAsync();
        Task<bool> ExcluirModalidadeAsync(int idModalidade);
    }
}
