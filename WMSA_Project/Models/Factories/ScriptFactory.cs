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

        public static Script GetComparativeScriptFromControls(ScriptControl baseControl, ScriptControl leftControl = null, ScriptControl rightControl = null)
        {
            baseControl.Script.ClearUnmatchedRequests();
            baseControl.Script.Transactions.ForEach((t) =>
            {
                if (leftControl != null)
                {
                    var leftControlTransaction = leftControl.Script.Transactions.First((leftT) => leftT.Name == t.Name);
                    ScriptTransactionsComparer.MatchRequests(t, leftControlTransaction, leftControl.LabelColor);
                }

                if (rightControl != null)
                {
                    var rightControlTransaction = rightControl.Script.Transactions.First((rightT) => rightT.Name == t.Name);
                    ScriptTransactionsComparer.MatchRequests(t, rightControlTransaction, rightControl.LabelColor);
                }
            });
            return baseControl.Script;
        }
    }
}
