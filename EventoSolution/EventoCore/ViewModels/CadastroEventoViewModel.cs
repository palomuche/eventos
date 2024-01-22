using System;
using System.Collections.Generic;
using System.Text;

namespace EventoCore.ViewModels
{
    public class CadastroEventoViewModel
    {
        public Guid? Id { get; set; }
        public bool Excluido { get; set; }
        public string? Titulo { get; set; }
        public string? Descricao { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Fim { get; set; }
        public Guid UsuarioId { get; set; }
    }
}
