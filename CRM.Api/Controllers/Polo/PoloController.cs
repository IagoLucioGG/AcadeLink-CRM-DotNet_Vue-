using CRM.DTOs.Polo_;
using CRM.Models.ResponseModel;
using CRM.Services.Polo_;
using Microsoft.AspNetCore.Mvc;

namespace CRM.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PoloController : ControllerBase
    {
        private readonly IPoloService _poloService;

        public PoloController(IPoloService poloService)
        {
            _poloService = poloService;
        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar([FromBody] PoloCadastroDto dto)
        {
            var result = await _poloService.CadastrarPoloAsync(dto);
            return Ok(ResponseModel<PoloRetornoDto>.Sucesso(result, "Polo cadastrado com sucesso."));
        }

        [HttpPut]
        public async Task<IActionResult> Editar([FromBody] PoloEdicaoDto dto)
        {
            var result = await _poloService.EditarPoloAsync(dto);
            if (result == null)
                return NotFound(ResponseModel<PoloRetornoDto>.Erro("Polo não encontrado."));

            return Ok(ResponseModel<PoloRetornoDto>.Sucesso(result, "Polo editado com sucesso."));
        }

        [HttpGet("{idPolo}")]
        public async Task<IActionResult> BuscarPorId(int idPolo)
        {
            var result = await _poloService.BuscarPorIdAsync(idPolo);
            if (result == null)
                return NotFound(ResponseModel<PoloRetornoDto>.Erro("Polo não encontrado."));

            return Ok(ResponseModel<PoloRetornoDto>.Sucesso(result, "Polo encontrado com sucesso."));
        }

        [HttpGet("listar")]
        public async Task<IActionResult> ListarTodos()
        {
            var result = await _poloService.ListarTodosAsync();
            return Ok(ResponseModel<List<PoloRetornoDto>>.Sucesso(result, "Polos listados com sucesso."));
        }

        [HttpDelete("{idPolo}")]
        public async Task<IActionResult> Excluir(int idPolo)
        {
            var sucesso = await _poloService.ExcluirPoloAsync(idPolo);
            if (!sucesso)
                return NotFound(ResponseModel<string>.Erro("Polo não encontrado."));

            return Ok(ResponseModel<string>.Sucesso("Polo excluído com sucesso."));
        }
    }
}
