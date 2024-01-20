using EventoCore.Context;
using EventoCore.Entities;
using EventoCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoCore.Repositories
{
    public class EventoRepository : IEventoRepository
    {
        protected readonly ApplicationContext _dbContext;

        public EventoRepository(ApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Evento> GetAll()
        {
            return _dbContext.Eventos.Where(w => !w.Excluido).AsEnumerable();
        }

        public Evento GetById(Guid id)
        {
            return _dbContext.Eventos.SingleOrDefault(t => t.Id == id);
        }
    }
}
