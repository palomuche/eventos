using EventoApi.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventoApi.Interfaces
{
    public interface IUsuarioRepository
    {
        IEnumerable<Usuario> GetAll();
        Usuario GetById(Guid id);
    }
}
