using EventoApi.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventoApi.Interfaces
{
    public interface IRepository<T> where T : EntidadeBase
    {
        T GetById(Guid id);

        T Add(T entity);

        T GetById(Guid Id, string[] includes);

        void Delete(T entity);

        void Delete(Guid Id);

        void Delete(IEnumerable<Guid> Ids);

        void Delete(IEnumerable<T> entities);

        void InsertOrReplace(T entity);
        void InsertOrReplace(T entity, Guid usuario);
        void InsertOrReplace(IEnumerable<T> entities);

        IEnumerable<T> GetAll();

        IEnumerable<T> GetAll(string[] includes);
    }
}
