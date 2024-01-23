using EventoCore.Interfaces;
using EventoWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace EventoWeb.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEventoRepository _eventoRepository;

        public HomeController(ILogger<HomeController> logger, IEventoRepository eventoRepository)
        {
            _logger = logger;
            _eventoRepository = eventoRepository;
        }

        public IActionResult Index()
        {
            if (HttpContext.Request.Cookies.TryGetValue("UserAuthToken", out string token))
            {
                var usuarioId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                ViewData["Eventos"] = _eventoRepository.GetByUsuario(token, usuarioId).OrderByDescending(o => o.Inicio);
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
