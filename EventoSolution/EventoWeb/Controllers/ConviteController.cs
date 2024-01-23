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

        public ConviteController(IConviteRepository conviteRepository)
        {
            _conviteRepository = conviteRepository;
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

    }
}
