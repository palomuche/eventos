using EventoCore.Entities;
using EventoCore.Interfaces;
using EventoCore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
                    var retorno = _usuarioRepository.RealizaLogin(model);
                    if (retorno.Sucesso)
                    {
                        return new RetornoViewModel
                        {
                            Sucesso = true,
                            Mensagem = retorno.Token
                        };
                    }
                    else
                    {
                        throw new Exception();
                    }
                }

                throw new Exception();
            }
            catch (Exception e)
            {
                return new RetornoViewModel
                {
                    Sucesso = false,
                    Mensagem = "Usuário ou senha incorretos"
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
