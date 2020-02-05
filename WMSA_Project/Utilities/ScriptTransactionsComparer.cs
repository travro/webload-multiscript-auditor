using System.Linq;
using System.Windows.Media;
using WMSA_Project.Models;


namespace WMSA_Project.Utilities
{
    public static class ScriptTransactionsComparer
    {
        public static bool CompareCount(Script scriptLeft, Script scriptRight)
        {
            return scriptLeft.Transactions.Count == scriptRight.Transactions.Count;
        }

        public static bool CompareEach(Script scriptLeft, Script scriptRight)
        {
            Transaction[] scriptLeftTrans = scriptLeft.Transactions.ToArray();
            Transaction[] scriptRightTrans = scriptRight.Transactions.ToArray();

            if (scriptLeftTrans.Length != scriptRightTrans.Length) return false;

            for (int i = 0; i < scriptLeftTrans.Length; i++)
            {
                if (scriptLeftTrans[i].Name != scriptRightTrans[i].Name)
                {
                    return false;
                }
            }
            return true;
        }

        public static bool CompareAll(Script scriptLeft, Script scriptRight)
        {
            return (CompareCount(scriptLeft, scriptRight) && CompareEach(scriptLeft, scriptRight)) ? true : false;
        }

        public static void MatchRequests(Transaction baseTrans, Transaction compTrans)
        {
            baseTrans.Requests.ForEach((baseReq) => baseReq.Matched = false);

            compTrans.Requests.ForEach((compReq) =>
            {
                var validRequest = baseTrans.Requests.FirstOrDefault((baseReq) => baseReq.Equals(compReq) && baseReq.Matched == false);   

                if (validRequest != null)
                {
                    validRequest.Matched = true;
                }
                else
                {
                    baseTrans.UnmatchedRequests.Add(new Request(compReq.Verb, compReq.URL, compReq.Visible));
                }
            });
        }
    }
}
