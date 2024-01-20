using EventoApi.Interfaces;
using EventoApi.Models;
using EventoApi.ViewModels;
using EventoWeb.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventoWeb.Controllers
{
    [Authorize]
    public class AutenticacaoController : Controller
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signManager;
        private readonly IUsuarioRepository _usuarioRepository;

        public AutenticacaoController(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager, IUsuarioRepository usuarioRepository)
        {
            _userManager = userManager;
            _signManager = signInManager;
            _usuarioRepository = usuarioRepository;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("/Login")]
        public IActionResult Login()
        {
            if (!_usuarioRepository.GetAll().Any())
            {
                var usuario = new Usuario
                {
                    UserName = "master",
                    Nome = "Master",
                    Email = "paulinho.mucheroni@gmail.com",
                };
                var senha = "Master@2024";
                var resultado = _userManager.CreateAsync(usuario, senha).Result;
            }

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public RetornoViewModel RealizaLogin(AutenticacaoViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = _userManager.FindByNameAsync(model.Login).Result;
                    if (user == null) throw new Exception("Usuário não encontrado");

                    var result = _signManager.PasswordSignInAsync(user.UserName, model.Senha, model.Lembrar, lockoutOnFailure: false).Result;

                    if (result.Succeeded)
                    {
                        return new RetornoViewModel
                        {
                            Sucesso = true
                        };
                    }
                    else
                    {
                        throw new Exception("Senha inválida");
                    }
                }

                throw new Exception("Falha no login");
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

        [HttpGet]
        [Route("/Sair")]
        public IActionResult Sair()
        {
            _signManager.SignOutAsync().Wait();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}
