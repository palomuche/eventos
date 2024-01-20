using EventoApi.Data;
using EventoApi.Interfaces;
using EventoApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoApi.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        protected readonly ApplicationContext _dbContext;

        public UsuarioRepository(ApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Usuario> GetAll()
        {
            return _dbContext.Usuarios.Where(w => !w.Excluido).AsEnumerable();
        }

        public Usuario GetById(Guid id)
        {
            return _dbContext.Usuarios.SingleOrDefault(t => t.Id == id);
        }
    }
}
