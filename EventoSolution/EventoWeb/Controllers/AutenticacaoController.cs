using EventoCore.Entities;
using EventoCore.Interfaces;
using EventoCore.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Security.Claims;

namespace EventoWeb.Controllers
{
    [Authorize]
    public class AutenticacaoController : Controller
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public AutenticacaoController(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        [HttpGet]
        [AllowAnonymous]
        //[Route("/Login")]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            if (!_usuarioRepository.GetAll().Any())
            {
                var usuario = new Usuario
                {
                    UserName = "master",
                    Nome = "Master",
                    Email = "paulinho.mucheroni@gmail.com",
                };
                var senha = "Master@2024";
                //var resultado = _userManager.CreateAsync(usuario, senha).Result;
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
                        var cookieOptions = new CookieOptions
                        {
                            HttpOnly = true, // Importante por razões de segurança
                            Secure = true, // Enviar apenas em HTTPS
                        };
                        Response.Cookies.Append("UserAuthToken", retorno.Token, cookieOptions);

                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, model.Login),
                            new Claim(ClaimTypes.NameIdentifier, retorno.UsuarioId.ToString())
                        };

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var authProperties = new AuthenticationProperties
                        {
                            // Configurações adicionais, como expiração do cookie
                        };

                        HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity),
                            authProperties);

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
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Autenticacao");
        }

        [AllowAnonymous]
        public IActionResult Cadastro()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult CadastrarUsuario(AutenticacaoViewModel model)
        {
            _usuarioRepository.CadastrarUsuario(model);

            return View();
        }
    }
}
