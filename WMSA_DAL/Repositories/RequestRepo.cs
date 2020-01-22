using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMSA_DAL.Models;
using WMSA_DAL.Interfaces;

namespace WMSA_DAL.Repositories
{
    public class RequestRepo : WLObjectRepo<Request>, IRepo<Request>
    {
        public RequestRepo()
        {
            _table = Context.Requests;
        }
        public int Delete(int entityId)
        {
            Context.Entry(new Request() { Id = entityId }).State = System.Data.Entity.EntityState.Deleted;
            return SaveChanges();
        }

        public Task<int> DeleteAsync(int entityId)
        {
            Context.Entry(new Request() { Id = entityId }).State = System.Data.Entity.EntityState.Deleted;
            return SaveChangesAsync();
        }
    }
}
