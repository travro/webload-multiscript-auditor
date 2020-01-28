using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using WMSA_Project.Models;
using WMSA_Project.Controls;
using WMSA_Project.Utilities;
using WMSA.Entities.Interfaces;

namespace WMSA_Project.Models.Factories
{
    public static class ScriptFactory
    {
        public static Script GetScriptFromFilePath(string filePath)
        {
            var script = new Script()
            {
                Name = filePath.Substring(filePath.LastIndexOf("\\") + 1)
            };

            try
            {
                using (FileStream _fStream = new FileStream(filePath, FileMode.Open))
                {
                    using (XmlReader _xReader = XmlReader.Create(_fStream))
                    {
                        XDocument _XDoc = XDocument.Load(_xReader);
                        script.Transactions = TransactionListFactory.GetTransactionsFromXDoc(_XDoc, script);
                    }
                }
            }
            catch (Exception fileStreamException)
            {
                throw fileStreamException;
            }
            return script;
        }

        public static Script GetComparativeScriptFromControls(ScriptControl baseControl)
        {
            baseControl.Script.ClearUnmatchedRequests();
            baseControl.Script.Transactions.ForEach((t) =>
            {
                if (baseControl.PrevComparison != null)
                {
                    var prevTransaction = baseControl.PrevComparison.Script.Transactions.First((prevT) => prevT.Name == t.Name);
                    ScriptTransactionsComparer.MatchRequests(t, prevTransaction);
                }
            });
            return baseControl.Script;
        }

        public static Script GetScriptFromDB(IScript script)
        {
            var newScript = new Script()
            {
                Id = script.Id,
                Name = script.Name,
                BuildVersion = script.BuildVersion,
                TestName = script.TestName,
                RecordedDate = script.RecordedDate,
                Transactions = script.Transactions as List<Transaction>,                
            };
            return newScript;
        }

    }
}
