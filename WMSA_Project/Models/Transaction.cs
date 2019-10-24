using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;


namespace WMSA_Project.Models
{
    public class Transaction
    {
        public int Id { get;  }
        public string Name { get; set; }
        public List<Request> Requests { get; set; }
        
        public Transaction(string name)
        {
            Name = name;
            Requests = new List<Request>();
        }
        public bool Equals(Transaction t)
        {
            return this.Name == t.Name;
        }

    }
}