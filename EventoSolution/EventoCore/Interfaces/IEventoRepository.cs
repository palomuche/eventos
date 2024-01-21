using EventoCore.Entities;
using EventoCore.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventoCore.Interfaces
{
    public interface IEventoRepository
    {
        IEnumerable<Evento> GetAll(string token);
        IEnumerable<Evento> GetByUsuario(string token, Guid usuarioId);
        Evento GetById(Guid id);
        Evento CadastrarEvento(AutenticacaoViewModel model, string token);
    }
}
