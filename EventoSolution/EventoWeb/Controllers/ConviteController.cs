using EventoCore.Entities;
using EventoCore.Interfaces;
using EventoCore.Repositories;
using EventoCore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EventoWeb.Controllers
{
    [Authorize]
    public class ConviteController : Controller
    {
        private readonly IConviteRepository _conviteRepository;
        private readonly IEventoRepository _eventoRepository;
        private readonly IUsuarioRepository _usuarioRepository;

        public ConviteController(IConviteRepository conviteRepository, IEventoRepository eventoRepository, IUsuarioRepository usuarioRepository)
        {
            _conviteRepository = conviteRepository;
            _eventoRepository = eventoRepository;
            _usuarioRepository = usuarioRepository;
        }

        public IActionResult Index()
        {
            if (HttpContext.Request.Cookies.TryGetValue("UserAuthToken", out string token))
            {
                var usuarioId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                ViewData["Convites"] = _conviteRepository.GetByUsuario(token, usuarioId).OrderByDescending(o => o.Status);
            }
            return View();
        }


        public IActionResult Cadastro(Guid eventoId)
        {
            if (HttpContext.Request.Cookies.TryGetValue("UserAuthToken", out string token))
            {
                var usuarioId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                ViewData["Usuarios"] = _usuarioRepository.GetAll().Where(u => u.Id != usuarioId);
                ViewData["Evento"] = _eventoRepository.GetById(token, (Guid)eventoId);
            }
            return View();
        }

        [HttpPut]
        public RetornoViewModel ConfirmarConvite(Guid id, bool confirmado)
        {

            if (HttpContext.Request.Cookies.TryGetValue("UserAuthToken", out string token))
            {
                return _conviteRepository.ConfirmarConvite(token, id, confirmado);
            }

            return new RetornoViewModel()
            {
                Sucesso = false,
                Mensagem = "Erro"
            };
        }

        [HttpPost]
        public RetornoViewModel EnviarConvite(Guid eventoId, Guid convidadoId)
        {

            if (HttpContext.Request.Cookies.TryGetValue("UserAuthToken", out string token))
            {
                return _conviteRepository.EnviarConvite(token, eventoId, convidadoId);
            }

            return new RetornoViewModel()
            {
                Sucesso = false,
                Mensagem = "Erro"
            };
        }

    }
}
