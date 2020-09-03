using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMSA.Entities.Interfaces;

namespace WMSA_Project.Models
{
    public class Script : IScript
    {
        public event EventHandler<ScriptResetEventArgs> ScriptReset;

        IEnumerable<ITransaction> IScript.Transactions
        {
            get => Transactions;
            set => Transactions = value as List<Transaction>;
        }
        public int Id { get; set; }
        public string TestName { get; set; }
        public string BuildVersion { get; set; }
        public string Name { get; set; }
        public DateTime RecordedDate { get; set; }
        public List<Transaction> Transactions { get; set; }
        public ScriptImportStrategy ImportStrategy { get; set; }


        public Script()
        {
            Transactions = new List<Transaction>();
        }
        public Script(string testName, string buildVers, string name, DateTime dateTime)
        {
            TestName = testName;
            BuildVersion = buildVers;
            Name = name;
            RecordedDate = dateTime;
        }
        public bool Contains(Transaction t) => Transactions.Exists(element => element.Name == t.Name);
        public bool Contains(string s) => Transactions.Exists(element => element.Name == s);
        public void ClearRequestsDropped()
        {
            Transactions.ForEach((t) => t.RequestsDropped.Clear());
            ScriptReset?.Invoke(this, new ScriptResetEventArgs() { Message = "This Script Reset" });
        } 
    }

    public class ScriptResetEventArgs : EventArgs
    {
        public string Message { get; set; }                
    }
}
