using EventoCore.Context;
using EventoCore.Entities;
using EventoCore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        [HttpGet("meus-eventos/{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<IEnumerable<Evento>>> GetEventoByUsuario(Guid id)
        {
            var eventos = await _context.Eventos.Where(w => !w.Excluido && w.UsuarioInclusaoId != null && (Guid)w.UsuarioInclusaoId == id).ToListAsync();

            if (eventos == null) return NotFound();

            return eventos;
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Evento>> PostEvento(Evento evento)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(new ValidationProblemDetails(ModelState)
                {
                    Title = "Um ou mais erros de validação ocorreram!",
                });
            }

            var validacao = ValidaEvento(evento);
            if (!validacao.Sucesso)
            {
                return ValidationProblem(new ValidationProblemDetails(ModelState)
                {
                    Title = validacao.Mensagem,
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

        private RetornoViewModel ValidaEvento(Evento evento)
        {
            try
            {
                if (evento.Inicio > evento.Fim) throw(new Exception("A data de fim deve ser maior que a de início!"));

                if(evento.UsuarioInclusaoId != null)
                {
                    var concomitante = _context.Eventos
                                        .Any(w => !w.Excluido 
                                             && w.UsuarioInclusaoId != null 
                                             && w.UsuarioInclusaoId == evento.UsuarioInclusaoId
                                             && (
                                                   //data inicio entre o inicio e fim
                                                   (w.Inicio <= evento.Inicio && evento.Inicio <= w.Fim)
                                                   //data fim entre o inicio e fim
                                                   || (w.Inicio <= evento.Fim && evento.Fim <= w.Fim)
                                                   //data inicio antes e fim depois
                                                   || (evento.Inicio <= w.Inicio && w.Fim <= evento.Fim)
                                                )
                                            );
                    if(concomitante) throw (new Exception("Existem eventos concomitantes!"));
                }

                return new RetornoViewModel
                {
                    Sucesso = true,
                };
            }
            catch (Exception e)
            {
                return new RetornoViewModel
                {
                    Sucesso = false,
                    Mensagem = e.Message
                };
            }
            

        }

    }
}
