﻿using System;
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
        public bool Contains(Transaction t)
        {
            return Transactions.Exists(element => element.Name == t.Name);
        }
        public bool Contains(string s)
        {
            return Transactions.Exists(element => element.Name == s);
        }
    }
}
