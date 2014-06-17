using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace EdeaDay.GestaoDeIdeias.Repository
{
    public abstract class Repository<C, T> : IRepository<T>
        where T : class
        where C : DbContext, new()
    {
        private C _context;

        protected C Context
        {
            get
            {
                return _context;
            }
        }

        public Repository(C context)
        {
            _context = context ?? Context;
        }

        public virtual IQueryable<T> All
        {
            get
            {
                return GetAll();
            }
        }

        public virtual IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>();

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return query;
        }

        public virtual IQueryable<T> GetAll()
        {
            IQueryable<T> query = _context.Set<T>();

            return query;
        }

        public virtual T Create()
        {
            return _context.Set<T>().Create();
        }

        public virtual T Find(params object[] keyValues)
        {
            return _context.Set<T>().Find(keyValues);
        }

        public virtual IQueryable<T> FindBy(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> query = _context.Set<T>().Where(predicate);

            return query;
        }

        public virtual void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public virtual void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public virtual void Edit(T entity)
        {
            _context.Entry(entity).State = System.Data.EntityState.Modified;
        }

        public virtual void Upsert(T entity, Func<T, bool> insertExpression)
        {
            if (insertExpression.Invoke(entity))
            {
                Add(entity);
            }
            else
            {
                Edit(entity);
            }
        }

        public virtual void Save()
        {
            _context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
                if (disposing)
                    _context.Dispose();

            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

    }
}