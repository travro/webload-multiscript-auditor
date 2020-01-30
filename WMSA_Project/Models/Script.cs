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
        public int Id { get; set; }
        public string TestName { get; set; }
        public string BuildVersion { get; set; }
        public string Name { get; set; }
        public DateTime RecordedDate { get; set; }
        public List<Transaction> Transactions { get; set; }
        IEnumerable<ITransaction> IScript.Transactions
        {
            get => Transactions; 
            set => Transactions = value as List<Transaction>;  
        }

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
        public void ClearUnmatchedRequests() => Transactions.ForEach((t) => t.UnmatchedRequests.Clear());
    }
}
