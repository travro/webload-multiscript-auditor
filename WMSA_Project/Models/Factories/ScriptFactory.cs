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
using IScript = WMSA.Entities.Interfaces.IScript;
using ScriptService = WMSA_DAL.Service.ScriptService;

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
            int adds = 0;
            int drops = 0;
            baseControl.Script.Transactions.ForEach((t) =>
            {
                if (baseControl.PrevComparison != null)
                {
                    var thisTransIndex = baseControl.Script.Transactions.IndexOf(t);

                    Transaction prevTransaction = baseControl.PrevComparison.Script.Transactions[thisTransIndex];
                    ScriptTransactionsComparer.MatchRequests(t, prevTransaction);
                    adds += t.Requests.Where(r => r.Matched == false).Count();
                    drops += t.UnmatchedRequests.Count();

                }
            });
            baseControl.TotalAdds = adds;
            baseControl.TotalDrops = drops;

            return baseControl.Script;
        }

        public static Script GetScriptFromDB(int scriptId)
        {
            IScript importedScript = ScriptService.GetScript(scriptId);

            var newScript = new Script()
            {
                Id = importedScript.Id,
                Name = importedScript.Name,
                BuildVersion = importedScript.BuildVersion,
                RecordedDate = importedScript.RecordedDate,
                TestName = importedScript.TestName
                
            };

            foreach (var importedTrans in importedScript.Transactions)
            {
                var newTrans = new Transaction(importedTrans.Name, newScript)
                {
                    Sleep = importedTrans.Sleep
                };

                foreach (var importedRequest in importedTrans.Requests)
                {
                    var newReq = new Request()
                    {
                        URL = importedRequest.URL,
                        Visible = true,
                        Correlations = new List<Correlation>()
                    };

                    RequestVerb v = RequestVerb.MISSING;
                    Enum.TryParse(importedRequest.Verb, out v);
                    newReq.Verb = v;

                    if (importedRequest.Correlations != null)
                    {
                        foreach (var importedCorrelation in importedRequest.Correlations)
                        {
                            var newCorr = new Correlation(importedCorrelation.Rule, importedCorrelation.OriginalValue);
                            newReq.Correlations.Add(newCorr);
                        } 
                    }

                    newTrans.Requests.Add(newReq);
                }
                newScript.Transactions.Add(newTrans);
            }

            return newScript;
        }
    }
}
