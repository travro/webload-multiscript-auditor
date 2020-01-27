using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMSA_DAL.Context;
using WMSA_DAL.Models;
using WMSA.Entities.Interfaces;

namespace WMSA_DAL.Service
{
    public class ScriptService
    {
        IScript _script;

        public ScriptService(IScript script)
        {
            _script = script;
        }

        //public IScript ImportScript(string testName, string buildVersion, string scriptName, DateTime recordedDate)
        //{

        //}
        public void SaveScript()
        {
            using (var context = new WLContext())
            {
                var newTest = context.Tests.Add(new Test() { test_name = _script.TestName, build_version = _script.BuildVersion });
                var newScript = context.Scripts.Add(new Script() { Name = _script.Name, RecordedDate = _script.RecordedDate, test_id = newTest.id });

                foreach (var trans in _script.Transactions)
                {
                    Transaction newTrans;

                    var queriedTransName = context.TransactionNames.FirstOrDefault(t => t.trans_name == trans.Name);

                    if (queriedTransName != null)
                    {
                        newTrans = context.Transactions.Add(new Transaction() { trans_nm_id = queriedTransName.id, script_id = newScript.Id });
                    }
                    else
                    {
                        var newTransName = context.TransactionNames.Add(new TransactionName() { trans_name = trans.Name });
                        newTrans = context.Transactions.Add(new Transaction() { trans_nm_id = newTransName.id, script_id = newScript.Id });
                    }

                    foreach(var req in trans.Requests)
                    {
                        var queriedReqVerb = context.RequestVerbs.FirstOrDefault(rV => rV.verb == req.Verb);

                        if(queriedReqVerb != null)
                        {
                            var newRequest = context.Requests.Add(new Request() { verb_id = queriedReqVerb.id, Parameters = req.Parameters, trans_id = newTrans.Id });
                        }
                    }
                }
                try
                {
                    context.SaveChanges();
                }
                catch(Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
