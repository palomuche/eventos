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
        Evento GetById(string token, Guid id);
        RetornoViewModel CadastrarEvento(CadastroEventoViewModel model, string token);
        RetornoViewModel EditarEvento(CadastroEventoViewModel model, string token);
        RetornoViewModel ExcluirEvento(string token, Guid id);
    }
}
