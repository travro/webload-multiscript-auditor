using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;


namespace WMSA_Project.Models
{
    public class Transaction
    {
        public int Id { get; }
        public string Name { get; set; }
        public List<Request> Requests { get; set; }
        public List<UnmatchedRequest> UnmatchedRequests { get; set; }
        public Script Script { get; }

        public Transaction(string name)
        {
            Name = name;
            Requests = new List<Request>();
            UnmatchedRequests = new List<UnmatchedRequest>();
        }
        public Transaction(string name, Script script)
        {
            Name = name;
            Script = script;
            Requests = new List<Request>();
            UnmatchedRequests = new List<UnmatchedRequest>();
        }
        public bool Equals(Transaction t)
        {
            return this.Name == t.Name;
        }
    }
}