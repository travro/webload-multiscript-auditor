using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using WMSA.Entities.Interfaces;


namespace WMSA_Project.Models
{
    public class Transaction : ITransaction
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Request> Requests { get; set; }
        public List<Request> UnmatchedRequests { get; set; }
        public Script Script { get; }
        IEnumerable<IRequest> ITransaction.Requests
        {
            get => Requests;
            set => Requests = value as List<Request>;
        }

        public Transaction(string name)
        {
            Name = name;
            Requests = new List<Request>();
            UnmatchedRequests = new List<Request>();
        }
        public Transaction(string name, Script script)
        {
            Name = name;
            Script = script;
            Requests = new List<Request>();
            UnmatchedRequests = new List<Request>();
        }
        public bool Equals(Transaction t)
        {
            return this.Name == t.Name;
        }
    }
}