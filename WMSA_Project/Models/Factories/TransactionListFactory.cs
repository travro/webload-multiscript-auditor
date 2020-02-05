using System.Xml.Linq;
using System.Linq;
using System.Collections.Generic;

namespace WMSA_Project.Models.Factories
{
    internal static class TransactionListFactory
    {
        public static List<Transaction> GetTransactionsFromXDoc(XDocument xDoc, Script script)
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

            //build list
            foreach (var jsPBE in jsParentBlockElements)
            {
                var transaction = new Transaction(ParseTransactionName(jsPBE), script);
                transaction.Sleep = ParseSleepTimeAfter(jsPBE.NextNode?.NextNode);
                transaction.Requests = RequestListFactory.GetRequestsFromXElement(jsPBE, false);
                transactions.Add(transaction);
            }
            return transactions;
        }
        #region helpermethods
        private static string ParseTransactionName(XElement element)
        {
            var beginTrans = "BeginTransaction::";

            string itemName = element
                .Element("Properties")
                .Element("PropertyPage")
                .Element("ItemName").Value;

            if (itemName.Contains(beginTrans))
            {
                return itemName.Substring(itemName.IndexOf(beginTrans) + beginTrans.Length);
            }
            else return "Transaction";
        }

        private static string ParseSleepTimeAfter(XNode node = null)
        {
            if (node != null)
            {
                var element = node as XElement;
                var sleepIndex = "Sleep(";

                string nodeScript = element
                    .Element("Properties")?
                    .Elements("PropertyPage").Where(e => e.Attribute("Name").Value == "JavaScript")
                    .FirstOrDefault()  //<---------does this need a parameter
                    .Element("NodeScript").Value;

                if (nodeScript != null && nodeScript.Contains(sleepIndex))
                {
                    int leftbound = nodeScript.IndexOf(sleepIndex) + sleepIndex.Length;
                    int rightbound = nodeScript.LastIndexOf(')');

                    return nodeScript.Substring(leftbound, rightbound - leftbound);
                }
            }
            return null;
        }
        #endregion
    }
}