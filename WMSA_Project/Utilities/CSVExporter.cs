using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMSA.Entities.Interfaces;
using CsvHelper;

namespace WMSA_Project.Utilities
{
    public static class CSVExporter
    {

        public static void ExportScriptCSV(IScript script, string fileName)
        {
            using (var writer = new StreamWriter(fileName, false))
            {
                writer.WriteLine("TestGroup,Script,Build,Date,Transaction,Sleep,Verb,URL");                

                foreach( var trans in script.Transactions)
                {
                    var record = $"{script.TestName},{script.Name},{script.BuildVersion},{script.RecordedDate},{trans.Name},{trans.Sleep},";

                    foreach(var req in trans.Requests)
                    {
                        writer.WriteLine($"{record},{req.Verb.ToString()},{req.URL}");
                    }
                }
            }
        }
    }
}
