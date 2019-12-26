using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMSA_Project.Models
{
    public class Script
    {
        public int Id { get; }
        public string TestName { get; set; }
        public string BuildVersion { get; set; }
        public string Name { get; set; }
        public DateTime RecordedDate { get; set; }
        public List<Transaction> Transactions { get; set; }

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
        public Script Clone()
        {
            var cloneScript = new Script(TestName, BuildVersion, Name, RecordedDate);
            cloneScript.Transactions = new List<Transaction>();

            Transactions.ForEach((t) =>
            {
                var trans = new Transaction(t.Name);

                t.Requests.ForEach((r) =>
                {
                    var req = new Request(r.Verb, r.Parameters, r.Visible);

                    if (r.Correlations != null)
                    {
                        req.Correlations = new List<Correlation>();
                        r.Correlations.ForEach((c) => req.Correlations.Add(new Correlation(c.Rule, c.OriginalValue)));
                    }

                    trans.Requests.Add(req);
                });

                cloneScript.Transactions.Add(trans);
            });

            return cloneScript;
        }
        public void ClearUnmatchedRequests() => Transactions.ForEach((t) => t.UnmatchedRequests.Clear());
    }
}
