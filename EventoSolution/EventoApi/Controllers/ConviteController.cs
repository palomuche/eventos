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
    [Route("api/convite")]
    public class ConviteController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public ConviteController(ApplicationContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Convite>>> Get()
        {
            return await _context.Convites.ToListAsync();
        }

        [HttpGet("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Convite>> GetConvite(Guid id)
        {
            var convite = await _context.Convites.FindAsync(id);

            if (convite == null) return NotFound();

            return convite;
        }

        [HttpGet("meus-convites/{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<IEnumerable<Convite>>> GetConvitesByUsuario(Guid id)
        {
            var convites = await _context.Convites
                                .Include(w => w.Convidado)
                                .Include(w => w.Evento)
                                .Include(w => w.Evento.UsuarioInclusao)
                                .Where(w => !w.Excluido && w.ConvidadoId == id)
                                .ToListAsync();

            if (convites == null) return NotFound();

            return convites;
        }

        [HttpPut("confirmar/{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> ConfirmarConvite(Guid id, Convite model)
        {
            if (model.Id != id) return BadRequest();

            var convite = _context.Convites.FirstOrDefault(e => e.Id == id);

            if (convite == null) return BadRequest();
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            convite.Status = model.Status;
            convite.Alteracao = DateTime.Now;

            _context.Entry(convite).State = EntityState.Modified;
            _context.Entry(convite).Property(x => x.Codigo).IsModified = false;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConviteExists(id))
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

        [HttpPost("enviar")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> EnviarConvite(CadastroConviteViewModel model)
        {

            var evento = _context.Eventos.FirstOrDefault(e => e.Id == model.EventoId);
            var convidado = _context.Usuarios.FirstOrDefault(e => e.Id == model.ConvidadoId);

            if (evento == null || convidado == null) return BadRequest("Evento ou Convidado não encontrado");

            if(_context.Convites.Any(c => !c.Excluido && c.ConvidadoId == model.ConvidadoId && c.EventoId == model.EventoId)) return BadRequest("Esse usuário já foi convidado!");

            var convite = new Convite() { 
                ConvidadoId = convidado.Id,
                EventoId = evento.Id,
                Status = Convite.StatusConvite.Pendente,
            };

            _context.Add(convite);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return BadRequest("Ocorreu um erro na API");
            }

            return Ok();
        }

        private bool ConviteExists(Guid id)
        {
            return (_context.Convites?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
