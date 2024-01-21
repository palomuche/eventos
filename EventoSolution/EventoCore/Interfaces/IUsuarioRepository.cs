using EventoCore.Entities;
using EventoCore.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventoCore.Interfaces
{
    public interface IUsuarioRepository
    {
        IEnumerable<Usuario> GetAll();
        Usuario GetById(Guid id);
        UsuarioLogadoViewModel RealizaLogin(AutenticacaoViewModel model);
        UsuarioLogadoViewModel CadastrarUsuario(AutenticacaoViewModel model);
    }
}
