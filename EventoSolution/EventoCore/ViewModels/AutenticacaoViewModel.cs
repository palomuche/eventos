using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventoCore.ViewModels
{
    public class AutenticacaoViewModel
    {
        [JsonProperty("login")]
        public string Login { get; set; }
        [JsonProperty("senha")]
        public string Senha { get; set; }
    }
}
