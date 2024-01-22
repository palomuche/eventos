using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace EventoCore.ViewModels
{
    public class RegisterUserViewModel
    {
        [JsonProperty("nome")]
        public string Nome { get; set; }

        [JsonProperty("email")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [EmailAddress(ErrorMessage = "O campo {0} está em formato inválido")]
        public string Email { get; set; }

        [JsonProperty("login")]
        public string Login { get; set; }

        [JsonProperty("senha")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 6)]
        public string Senha { get; set; }

        [JsonProperty("senhaConfirmacao")]
        [Compare("Senha", ErrorMessage = "As senhas não conferem")]
        public string SenhaConfirmacao { get; set; }

    }
}
