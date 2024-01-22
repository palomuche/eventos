using EventoCore.Entities;
using EventoCore.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace EventoApi.Controllers
{
    [ApiController]
    [Route("api/conta")]
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<Usuario> _signInManager;
        private readonly UserManager<Usuario> _userManager;
        private readonly JwtSettings _jwtSettings;

        public AuthController(SignInManager<Usuario> signInManager,
                              UserManager<Usuario> userManager,
                              IOptions<JwtSettings> jwtSettings)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtSettings = jwtSettings.Value;
        }

        [HttpPost("cadastro")]
        public async Task<ActionResult> Registrar(RegisterUserViewModel registerUser)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var user = new Usuario
            {
                Nome = registerUser.Nome,
                Email = registerUser.Email,
                EmailConfirmed = true,
                UserName = registerUser.Login,
            };

            var result = await _userManager.CreateAsync(user, registerUser.Senha);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                //return Ok(await GerarJwt(user.Email));
                //return Ok();
                return Ok(GerarJwt());
            }

            return Problem("Falha ao registrar usuário");
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] AutenticacaoViewModel loginUser)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var user = _userManager.FindByNameAsync(loginUser.Login).Result;
            if (user == null) throw new Exception("Usuário ou senha incorretos");

            var result = _signInManager.PasswordSignInAsync(user.UserName, loginUser.Senha, false, lockoutOnFailure: false).Result;

            if (result.Succeeded)
            {
                return Ok(new UsuarioLogadoViewModel { Token = GerarJwt(), UsuarioId = user.Id, UsuarioNome = user.Nome, Sucesso = true});
            }

            return Problem("Usuário ou senha incorretos");
        }

        private string GerarJwt()
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Segredo);

            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _jwtSettings.Emissor,
                Audience = _jwtSettings.Audiencia,
                Expires = DateTime.UtcNow.AddHours(_jwtSettings.ExpiracaoHoras),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });

            var encodedToken = tokenHandler.WriteToken(token);

            return encodedToken;
        }

    }
}
