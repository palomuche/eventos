using System.ComponentModel.DataAnnotations;

namespace EventoCore.ViewModels
{
    public class UsuarioLogadoViewModel
    {
        public string Token { get; set; }

        public bool Sucesso { get; set; }

        public Guid UsuarioId { get; set; }

        public string UsuarioNome { get; set; }
    }
}
