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

        //public IScript GetScript(int scriptID)
        //{

        //}
        public void SaveScript()
        {
            using (var context = new WLContext())
            {
                using (var contextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var newTest = context.Tests.Add(new Test() { test_name = _script.TestName, build_version = _script.BuildVersion });
                        context.SaveChanges();

                        var newScript = context.Scripts.Add(new Script() { Name = _script.Name, RecordedDate = _script.RecordedDate, test_id = newTest.id });
                        context.SaveChanges();

                        foreach (var trans in _script.Transactions)
                        {
                            Transaction newTrans;

                            var queriedTransName = context.TransactionNames.FirstOrDefault(t => t.trans_name == trans.Name);

                            if (queriedTransName != null)
                            {
                                newTrans = context.Transactions.Add(new Transaction() { trans_nm_id = queriedTransName.id, script_id = newScript.Id });
                                context.SaveChanges();
                            }
                            else
                            {
                                var newTransName = context.TransactionNames.Add(new TransactionName() { trans_name = trans.Name });
                                context.SaveChanges();

                                newTrans = context.Transactions.Add(new Transaction() { trans_nm_id = newTransName.id, script_id = newScript.Id });
                                context.SaveChanges();
                            }

                            foreach (var req in trans.Requests)
                            {
                                var queriedReqVerb = context.RequestVerbs.FirstOrDefault(rV => rV.verb == req.Verb);

                                if (queriedReqVerb != null)
                                {
                                    var newRequest = context.Requests.Add(new Request() { verb_id = queriedReqVerb.id, Parameters = req.Parameters, trans_id = newTrans.Id });
                                    context.SaveChanges();
                                }
                            }
                        }
                        contextTransaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        contextTransaction.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        contextTransaction.Dispose();
                    }
                }
            }
        }
    }
}
