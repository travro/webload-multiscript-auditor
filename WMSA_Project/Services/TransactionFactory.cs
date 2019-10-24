using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using WMSA_Project.Models;
using System;
using System.Collections.Generic;

namespace WMSA_Project.Services
{
    public static class TransactionFactory
    {
        public static List<Transaction> GetTransactionsFromXDoc(XDocument xDoc)
        {
            var transactions = new List<Transaction>();
            var jsParentBlockElements = xDoc
                .Descendants("Agenda")
                .Elements("JavaScriptObject")
                .Where(element => element.Element("Properties")
                .Element("PropertyPage")
                .Element("ItemName")
                .Value
                .Contains("BeginTransaction::"));

            
            foreach (var jsPBE in jsParentBlockElements)
            {
                var transaction = new Transaction(ParseTransactionName(jsPBE));
                transaction.Requests = RequestFactory.GetRequestFromXElement(jsPBE);
            }
            return transactions;
        }
        private static string ParseTransactionName(XElement element)
        {
            string itemName = element
                .Element("Properties")
                .Element("PropertyPage")
                .Element("ItemName").Value;
            string beginTrans = "BeginTransaction::";

            if (itemName.Contains(beginTrans))
            {
                return itemName.Substring(itemName.IndexOf(beginTrans) + beginTrans.Length);
            }
            else return "Transaction";
        }
    }
}