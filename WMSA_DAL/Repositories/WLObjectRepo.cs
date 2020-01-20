using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMSA_DAL.Context;

namespace WMSA_DAL.Repositories
{
    public abstract class WLObjectRepo<TEntity>: IDisposable where TEntity: class
    {        
        protected DbSet<TEntity> _table;
        private bool _disposed = false;

        public WLContext Context { get; }

        public WLObjectRepo()
        {
            Context = new WLContext();
        }

        public int Add(TEntity entity)
        {
            _table.Add(entity);
            return SaveChanges();
        }
        public Task<int> AddAsync(TEntity entity)
        {
            _table.Add(entity);
            return SaveChangesAsync();
        }
        public int AddRange(IList<TEntity> eRange)
        {
            _table.AddRange(eRange);
            return SaveChanges();
        }
        public Task<int> AddRangeAsync(IList<TEntity> eRange)
        {
            _table.AddRange(eRange);
            return SaveChangesAsync();
        }
        public int Delete(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Deleted;
            return SaveChanges();
        }
        public Task<int> DeleteAsync(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Deleted;
            return SaveChangesAsync();
        }
        public TEntity GetOne(int? id) => _table.Find(id);
        public Task<TEntity> GetOneAsync(int? id) => _table.FindAsync(id);
        public List<TEntity> GetAll() => _table.ToList();
        public Task<List<TEntity>> GetAllAsync() => _table.ToListAsync();

        protected int SaveChanges()
        {
            try
            {
                return Context.SaveChanges();
            }
            catch(Exception)
            {
                throw;
            }
        }
        protected Task<int> SaveChangesAsync()
        {
            try
            {
                return Context.SaveChangesAsync();
            }
            catch(Exception)
            {
                throw;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                Context.Dispose();
            }
            _disposed = true;
        }
    }
}
