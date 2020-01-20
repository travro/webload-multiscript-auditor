using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMSA_DAL.Interfaces
{
    public interface IRepo<TEntity> where TEntity: class
    {
        int Add(TEntity entity);
        Task<int> AddAsync(TEntity entity);
        int AddRange(IList<TEntity> eRange);
        Task<int> AddRangeAsync(IList<TEntity> eRange);
        int Delete(int id);
        Task<int> DeleteAsync(int id);
        int Delete(TEntity entity);
        Task<int> DeleteAsync(TEntity entity);
        List<TEntity> GetAll();
        Task<List<TEntity>> GetAllAsync();
        TEntity GetOne(int? id);
        Task<TEntity> GetOneAsync(int? id);
    }
}
