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
    public class EventoController : Controller
    {
        private readonly IEventoRepository _eventoRepository;

        public EventoController(IEventoRepository eventoRepository)
        {
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

        public IActionResult Cadastro(Guid? id)
        {

            if (HttpContext.Request.Cookies.TryGetValue("UserAuthToken", out string token))
            {
                if (id != null)
                {
                    ViewData["Evento"] = _eventoRepository.GetById(token, (Guid)id);
                }
            }
            return View();
        }

        [HttpPost]
        public RetornoViewModel CadastrarEvento(CadastroEventoViewModel model)
        {
            try
            {
                if (HttpContext.Request.Cookies.TryGetValue("UserAuthToken", out string token))
                {
                    if (model.Id == null)
                    {
                        var usuarioId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                        model.UsuarioId = usuarioId;
                        return _eventoRepository.CadastrarEvento(model, token);
                    }
                    else
                    {
                        return _eventoRepository.EditarEvento(model, token);
                    }
                }

                return new RetornoViewModel() { Mensagem = "Erro", Sucesso = false };
            }
            catch (Exception e)
            {
                return new RetornoViewModel() { Mensagem = "Erro: " + e.Message, Sucesso = false };
            }
        }

        [HttpDelete]
        public RetornoViewModel ExcluirEvento(Guid id)
        {

            if (HttpContext.Request.Cookies.TryGetValue("UserAuthToken", out string token))
            {
                return _eventoRepository.ExcluirEvento(token, id);
            }

            return new RetornoViewModel()
            {
                Sucesso = false,
                Mensagem = "Erro"
            };
        }
    }
}
