using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMSA_DAL.Models;
using WMSA_DAL.Interfaces;

namespace WMSA_DAL.Repositories
{
    public class TestRepo : WLObjectRepo<Test>, IRepo<Test>
    {
        public TestRepo() : base()
        {
            _table = Context.Tests;
        }

        public int Delete(int entityId)
        {
            Context.Entry(new Test() { id = entityId }).State = System.Data.Entity.EntityState.Deleted;
            return SaveChanges();
        }

        public Task<int> DeleteAsync(int entityId)
        {
            Context.Entry(new Test() { id = entityId }).State = System.Data.Entity.EntityState.Deleted;
            return SaveChangesAsync();
        }

        public int GetTestId(string name)
        {
            return _table.FirstOrDefault(t => t.test_name == name).id;
        }
    }
}
