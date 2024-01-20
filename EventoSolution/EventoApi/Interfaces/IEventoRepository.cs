using EventoApi.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventoApi.Interfaces
{
    public interface IEventoRepository
    {
        IEnumerable<Evento> GetAll();
        Evento GetById(Guid id);
    }
}
