using EventoCore.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventoWeb.Controllers
{
    [Authorize]
    public class EventoController : Controller
    {
        private readonly IEventoRepository _eventoRepository;

        public EventoController(IEventoRepository eventoRepository)
        {
            _eventoRepository = eventoRepository;
        }

        public IActionResult Index()
        {
            ViewData["Eventos"] = _eventoRepository.GetAll();
            return View();
        }
    }
}
