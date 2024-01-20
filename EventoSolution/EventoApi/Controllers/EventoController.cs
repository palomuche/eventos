using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using EventoCore.Context;
using EventoCore.Entities;

namespace EventoApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/evento")]
    public class EventoController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public EventoController(ApplicationContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Evento>>> Get()
        {
            return await _context.Eventos.ToListAsync();
        }

        [HttpGet("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Evento>> GetEvento(Guid id)
        {
            var evento = await _context.Eventos.FindAsync(id);

            if (evento == null) return NotFound();

            return evento;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Evento>> PostEvento(Evento evento)
        {
            if (!ModelState.IsValid)
            {
                //return BadRequest(ModelState);
                //return ValidationProblem(ModelState);

                return ValidationProblem(new ValidationProblemDetails(ModelState)
                {
                    Title = "Um ou mais erros de validação ocorreram!",
                });
            }

            _context.Eventos.Add(evento);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEvento), new { evento.Id }, evento);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> PutEvento(Guid id, Evento evento)
        {
            if (id != evento.Id) return BadRequest();
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            //_context.Eventos.Update(evento);  
            _context.Entry(evento).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }


            return NoContent();
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> DeleteEvento(int id)
        {
            var evento = await _context.Eventos.FindAsync(id);

            if (evento == null) return NotFound();

            _context.Eventos.Remove(evento);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EventoExists(Guid id)
        {
            return (_context.Eventos?.Any(e => e.Id == id)).GetValueOrDefault();
        }

    }
}
