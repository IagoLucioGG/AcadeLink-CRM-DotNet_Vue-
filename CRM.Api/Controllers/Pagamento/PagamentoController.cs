using CRM.DTOs.Pagamento_;
using CRM.Models.ResponseModel;
using CRM.Services.Pagamento_;
using Microsoft.AspNetCore.Mvc;

namespace CRM.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PagamentoController : ControllerBase
    {
        private readonly IPagamentoService _pagamentoService;

        public PagamentoController(IPagamentoService pagamentoService)
        {
            _pagamentoService = pagamentoService;
        }

        [HttpPost("cadastrar")]
        public async Task<ActionResult<ResponseModel<PagamentoRetornoDto>>> Cadastrar([FromBody] PagamentoCadastroDto dto)
        {
            var pagamento = await _pagamentoService.CadastrarAsync(dto);
            return Ok(ResponseModel<PagamentoRetornoDto>.Sucesso(pagamento, "Pagamento cadastrado com sucesso."));
        }

        [HttpPut("editar")]
        public async Task<ActionResult<ResponseModel<PagamentoRetornoDto>>> Editar([FromBody] PagamentoEdicaoDto dto)
        {
            var pagamento = await _pagamentoService.EditarAsync(dto);
            if (pagamento == null)
                return NotFound(ResponseModel<PagamentoRetornoDto>.Erro("Pagamento não encontrado."));

            return Ok(ResponseModel<PagamentoRetornoDto>.Sucesso(pagamento, "Pagamento editado com sucesso."));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseModel<PagamentoRetornoDto>>> BuscarPorId(int id)
        {
            var pagamento = await _pagamentoService.BuscarPorIdAsync(id);
            if (pagamento == null)
                return NotFound(ResponseModel<PagamentoRetornoDto>.Erro("Pagamento não encontrado."));

            return Ok(ResponseModel<PagamentoRetornoDto>.Sucesso(pagamento));
        }

        [HttpGet("listar")]
        public async Task<ActionResult<ResponseModel<List<PagamentoRetornoDto>>>> ListarTodos()
        {
            var lista = await _pagamentoService.ListarTodosAsync();
            return Ok(ResponseModel<List<PagamentoRetornoDto>>.Sucesso(lista));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponseModel<bool>>> Excluir(int id)
        {
            var sucesso = await _pagamentoService.ExcluirAsync(id);
            if (!sucesso)
                return NotFound(ResponseModel<bool>.Erro("Pagamento não encontrado."));

            return Ok(ResponseModel<bool>.Sucesso(true, "Pagamento excluído com sucesso."));
        }
    }
}
