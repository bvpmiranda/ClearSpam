using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ClearSpam.Domain.Interfaces
{
    public interface IRepository
    {
        IEnumerable<TEntity> Get<TEntity>() where TEntity : class;
        TEntity Get<TEntity>(object id) where TEntity : class;
        IEnumerable<TEntity> Get<TEntity>(IEnumerable<object> ids) where TEntity : class;

        IEnumerable<TEntity> Get<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;
        bool Any<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;
        TEntity FirstOrDefault<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;
        TEntity SingleOrDefault<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;
        IEnumerable<int> Ids<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;

        void Add<TEntity>(TEntity entity) where TEntity : class;
        void AddRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;

        void Remove<TEntity>(TEntity entity) where TEntity : class;
        void RemoveRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;

        int SaveChanges();
    }
}