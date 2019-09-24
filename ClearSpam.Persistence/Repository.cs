using ClearSpam.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ClearSpam.Persistence
{
    public class Repository : IRepository, IDisposable
    {
        private readonly bool _transactional;
        internal readonly ClearSpamContext ClearSpamContext;

        public Repository(ClearSpamContext clearSpamContext, bool transactional = true)
        {
            ClearSpamContext = clearSpamContext;
            _transactional = transactional;
        }

        public void Dispose()
        {
            ClearSpamContext.Dispose();
        }

        public IEnumerable<TEntity> Get<TEntity>()
            where TEntity : class
        {
            return Entities<TEntity>().ToList();
        }

        public TEntity Get<TEntity>(object id)
            where TEntity : class
        {
            return Entities<TEntity>().Find(id);
        }

        public IEnumerable<TEntity> Get<TEntity>(IEnumerable<object> ids)
            where TEntity : class
        {
            return Entities<TEntity>().Where(x => ids.Contains(((IEntity)x).Id)).ToList();
        }

        public IEnumerable<TEntity> Get<TEntity>(Expression<Func<TEntity, bool>> predicate)
            where TEntity : class
        {
            return Entities<TEntity>().Where(predicate).ToList();
        }

        public bool Any<TEntity>(Expression<Func<TEntity, bool>> predicate)
            where TEntity : class
        {
            return Entities<TEntity>().Any(predicate);
        }

        public TEntity FirstOrDefault<TEntity>(Expression<Func<TEntity, bool>> predicate)
            where TEntity : class
        {
            return Entities<TEntity>().FirstOrDefault(predicate);
        }

        public TEntity SingleOrDefault<TEntity>(Expression<Func<TEntity, bool>> predicate)
            where TEntity : class
        {
            return Entities<TEntity>().SingleOrDefault(predicate);
        }

        public IEnumerable<int> Ids<TEntity>(Expression<Func<TEntity, bool>> predicate)
            where TEntity : class
        {
            return Entities<TEntity>().Where(predicate).Select(x => ((IEntity)x).Id).ToList();
        }

        public void Add<TEntity>(TEntity entity)
            where TEntity : class
        {
            Entities<TEntity>().Add(entity);
        }

        public void AddRange<TEntity>(IEnumerable<TEntity> entities)
            where TEntity : class
        {
            Entities<TEntity>().AddRange(entities);
        }

        public void Remove<TEntity>(TEntity entity)
            where TEntity : class
        {
            Entities<TEntity>().Remove(entity);
        }

        public void RemoveRange<TEntity>(IEnumerable<TEntity> entities)
            where TEntity : class
        {
            Entities<TEntity>().RemoveRange(entities);
        }

        public int SaveChanges()
        {
            var transaction = !_transactional ? null : ClearSpamContext.Database.BeginTransaction();

            var result = ClearSpamContext.SaveChanges();

            transaction?.Commit();

            return result;
        }

        private DbSet<TEntity> Entities<TEntity>()
            where TEntity : class
        {
            return ClearSpamContext.Set<TEntity>();
        }
    }
}