using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace kazue.Controllers
{
    [Route("api/fila")]
    [ApiController]
    public class FilaController : ControllerBase
    {
        private readonly KazueContext _context;

        public FilaController(KazueContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> AdicionarNaFila([FromBody] InserirClienteRequest req)
        {
            var cliente = new ClienteEntidade
            {
                Nome = req.Nome,
                Servico = req.Servico,
                BarbeiroPreferido = req.BarbeiroPreferido ?? "Não",
                RegistradoEm = req.RegistradoEm,
                Status = req.Status,
            };

            _context.Fila.Add(cliente);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(AdicionarNaFila), new { id = cliente.Id }, cliente);
        }

        [HttpGet]
        public async Task<IActionResult> ListarFila()
        {
            var queue = await _context.Fila.OrderBy(q => q.RegistradoEm).ToListAsync();
            return Ok(queue);
        }

        [HttpGet("ativa")]
        public async Task<IActionResult> ListarFilaAtiva(DateTime data)
        {
            var queue = await _context.Fila
                .Where(cliente => cliente.RegistradoEm.Date == data.Date && (cliente.Status == "Aguardando" || cliente.Status == "Em andamento"))
                .OrderBy(q => q.RegistradoEm)
                .ToListAsync();
            return Ok(queue);
        }

        [HttpGet("preferencia")]
        public async Task<IActionResult> ListarPreferenciaComBarbeiro(DateTime data, string barbeiro)
        {
            var queue = await _context.Fila
                .Where(cliente => cliente.RegistradoEm.Date == data.Date 
                    && cliente.Status == "Aguardando"
                    && cliente.BarbeiroPreferido == barbeiro)
                .OrderBy(q => q.RegistradoEm)
                .ToListAsync();
            return Ok(queue);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoverDaFila(long id)
        {
            var cliente = await _context.Fila.FindAsync(id);
            if (cliente == null) return NotFound();
            _context.Fila.Remove(cliente);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("finalizado/{id}")]
        public async Task<IActionResult> AtualizarStatusFinalizado(long id)
        {
            var cliente = await _context.Fila.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            else
            {
                cliente.Status = "Finalizado";
            }
                
            _context.Fila.Update(cliente);
            await _context.SaveChangesAsync();
            return Ok(cliente);
        }

        [HttpPut("em-andamento/{id}")]
        public async Task<IActionResult> AtualizarStatusAndamento(long id)
        {
            var cliente = await _context.Fila.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            else
            {
                cliente.Status = "Em andamento";
            }

            _context.Fila.Update(cliente);
            await _context.SaveChangesAsync();
            return Ok(cliente);
        }
    }
}
