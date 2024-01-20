using EventoCore.Context;
using EventoCore.Entities;
using EventoCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoCore.Repositories
{
    public class Repository<T> : IRepository<T> where T : EntidadeBase
    {
        protected readonly ApplicationContext _dbContext;
        public Repository(ApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }

        public T Add(T entity)
        {
            _dbContext.Set<T>().Add(entity);
            _dbContext.SaveChanges();

            return entity;
        }

        public IEnumerable<T> GetAll()
        {
            try
            {
                return _dbContext.Set<T>().Where(t => !t.Excluido);
            }
            catch (Exception e)
            {
                throw new Exception(e.InnerException.Message);
            }

        }

        public IEnumerable<T> GetAll(string[] includes)
        {
            var dados = _dbContext.Set<T>().Where(t => !t.Excluido);


            IQueryable<T> dadosCompletos = null;

            foreach (var include in includes)
            {
                dadosCompletos = dados.Include(include);
            }

            return dadosCompletos.AsEnumerable();
        }

        public T GetById(Guid id)
        {
            return _dbContext.Set<T>().SingleOrDefault(t => t.Id == id);
        }

        public T GetById(Guid Id, string[] includes)
        {
            var dados = _dbContext.Set<T>();
            IQueryable<T> dadosCompletos = null;

            foreach (var include in includes)
            {
                dadosCompletos = dados.Include(include);
            }
            return dados.SingleOrDefault(t => t.Id == Id) ?? throw new Exception("Registro não encontrado");
        }

        public void Delete(T entity)
        {
            Delete(entity.Id);
        }

        public void Delete(Guid Id)
        {
            var Entity = _dbContext.Set<T>().Find(Id) ?? throw new Exception("Registro não encontrado");

            Entity.Excluido = true;
            Entity.Alteracao = DateTime.Now;

            _dbContext.Set<T>().Update(Entity).Property(x => x.Codigo).IsModified = false;
            _dbContext.SaveChanges();
        }

        public void Delete(IEnumerable<Guid> Ids)
        {
            try
            {
                using (var context = _dbContext)
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            foreach (var item in Ids)
                            {
                                var Entity = context.Set<T>().Find(item) ?? throw new Exception("Registro não encontrado");

                                Entity.Excluido = true;
                                Entity.Alteracao = DateTime.Now;

                                context.Set<T>().Update(Entity).Property(x => x.Codigo).IsModified = false;
                            }
                            context.SaveChanges();
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw new Exception(ex.Message);
                        }
                    }
                }

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void Delete(IEnumerable<T> entities)
        {
            try
            {
                using (var context = _dbContext)
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            foreach (var item in entities)
                            {
                                var Entity = context.Set<T>().Find(item.Id) ?? throw new Exception("Registro não encontrado");

                                Entity.Excluido = true;
                                Entity.Alteracao = DateTime.Now;

                                _dbContext.Set<T>().Update(Entity).Property(x => x.Codigo).IsModified = false;
                            }
                            context.SaveChanges();
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw new Exception(ex.Message);
                        }
                    }
                }

            }
            catch (Exception e)
            {
                throw new Exception(e.InnerException.Message);
            }
        }

        public void InsertOrReplace(T entity)
        {
            try
            {
                var Entity = _dbContext.Set<T>().AsNoTracking().SingleOrDefault(e => e.Id == entity.Id);

                entity.Alteracao = DateTime.Now;
                entity.Excluido = false;

                if (Entity == null)
                {
                    entity.Inclusao = DateTime.Now;
                    _dbContext.Set<T>().Add(entity);
                }
                else
                {
                    _dbContext.Set<T>().Update(entity).Property(x => x.Codigo).IsModified = false;
                }
                _dbContext.SaveChanges();

            }
            catch (Exception e)
            {
                throw new Exception(e.InnerException.Message);
            }
        }

        public void InsertOrReplace(T entity, Guid usuario)
        {
            try
            {
                var Entity = _dbContext.Set<T>().AsNoTracking().SingleOrDefault(e => e.Id == entity.Id);

                entity.Alteracao = DateTime.Now;
                entity.Excluido = false;
                entity.UsuarioAlteracaoId = usuario;

                if (Entity == null)
                {
                    entity.UsuarioInclusaoId = usuario;
                    entity.Inclusao = DateTime.Now;
                    _dbContext.Set<T>().Add(entity);
                }
                else
                {
                    _dbContext.Set<T>().Update(entity).Property(x => x.Codigo).IsModified = false;
                }
                _dbContext.SaveChanges();

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void InsertOrReplace(IEnumerable<T> entities)
        {
            try
            {
                using (var context = _dbContext)
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        try
                        {

                            foreach (var item in entities)
                            {
                                var Entity = context.Set<T>().AsNoTracking().SingleOrDefault(e => e.Id == item.Id);

                                item.Alteracao = DateTime.Now;
                                item.Excluido = false;

                                if (Entity == null)
                                {
                                    context.Set<T>().Add(item);
                                }
                                //se encoutrou
                                else
                                {
                                    context.Set<T>().Update(item).Property(x => x.Codigo).IsModified = false;
                                }
                            }
                            context.SaveChanges();
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw new Exception(ex.Message);
                        }
                    }
                }

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

    }
}
