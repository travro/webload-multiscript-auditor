using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMSA_DAL.Models;
using WMSA_DAL.Interfaces;

namespace WMSA_DAL.Repositories
{
    public class ScriptRepo : WLObjectRepo<Script>, IRepo<Script>
    {
        public ScriptRepo()
        {
            _table = Context.Scripts;
        }
        public int Delete(int entityId)
        {
            Context.Entry(new Script() { Id = entityId }).State = System.Data.Entity.EntityState.Deleted;
            return SaveChanges();
        }

        public Task<int> DeleteAsync(int entityId)
        {
            Context.Entry(new Script() { Id = entityId }).State = System.Data.Entity.EntityState.Deleted;
            return SaveChangesAsync();
        }

        public List<Script> GetScriptsByTestName(string testName)
        {
            try
            {
                using (var repo = new TestRepo())
                {
                    var testId = repo.GetTestId(testName);
                    return _table.Where(s => s.test_id == testId).ToList();
                }
            }
            catch 
            {
                throw new NullReferenceException("No scripts found under the currently listed Test group name");
            }
        }
    }
}
