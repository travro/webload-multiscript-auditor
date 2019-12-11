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
                        script.Transactions = TransactionListFactory.GetTransactionsFromXDoc(_XDoc);
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
            return baseControl.Script;
        }
    }
}
