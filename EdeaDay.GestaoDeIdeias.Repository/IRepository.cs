using System;
using System.Linq;
using System.Linq.Expressions;

namespace EdeaDay.GestaoDeIdeias.Repository
{

    public interface IRepository<T> : IDisposable where T : class
    {
        void Upsert(T entity, Func<T, bool> insertExpression);
        IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties);
        IQueryable<T> All { get; }
        T Find(params object[] keyValues);
        IQueryable<T> GetAll();
        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);
        T Create();
        void Add(T entity);
        void Delete(T entity);
        void Edit(T entity);
        void Save();
    }
}