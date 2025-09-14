using CRM.DTOs.FormaPagamento_;
using CRM.DTOs.Pagamento_;
using CRM.Models.Pagamento_;
using CRM.Models.ResponseModel;
using CRM.Services.Pagamento_;
using Microsoft.AspNetCore.Mvc;

namespace CRM.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FormaPagamentoController : ControllerBase
    {
        private readonly IFormaPagamentoService _service;

        public FormaPagamentoController(IFormaPagamentoService service)
        {
            _service = service;
        }

        [HttpPost("cadastrar")]
        public async Task<ActionResult<ResponseModel<FormaPagamento>>> Cadastrar([FromBody] FormaPagamentoRequestDTO dto)
        {
            var formaPagamento = await _service.CadastrarAsync(dto);
            return Ok(ResponseModel<FormaPagamento>.Sucesso(formaPagamento, "Forma de pagamento cadastrada com sucesso."));
        }

        [HttpPut("editar")]
        public async Task<ActionResult<ResponseModel<FormaPagamento>>> Editar([FromBody] FormaPagamento dto)
        {
            var formaPagamento = await _service.EditarAsync(dto);
            if (formaPagamento == null)
                return NotFound(ResponseModel<FormaPagamento>.Erro("Forma de pagamento não encontrada."));

            return Ok(ResponseModel<FormaPagamento>.Sucesso(formaPagamento, "Forma de pagamento editada com sucesso."));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseModel<FormaPagamento>>> BuscarPorId(int id)
        {
            var formaPagamento = await _service.BuscarPorIdAsync(id);
            if (formaPagamento == null)
                return NotFound(ResponseModel<FormaPagamento>.Erro("Forma de pagamento não encontrada."));

            return Ok(ResponseModel<FormaPagamento>.Sucesso(formaPagamento));
        }

        [HttpGet("listar")]
        public async Task<ActionResult<ResponseModel<List<FormaPagamento>>>> ListarTodos()
        {
            var lista = await _service.ListarTodosAsync();
            return Ok(ResponseModel<List<FormaPagamento>>.Sucesso(lista));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponseModel<bool>>> Excluir(int id)
        {
            var sucesso = await _service.ExcluirAsync(id);
            if (!sucesso)
                return NotFound(ResponseModel<bool>.Erro("Forma de pagamento não encontrada."));

            return Ok(ResponseModel<bool>.Sucesso(true, "Forma de pagamento excluída com sucesso."));
        }
    }
}
