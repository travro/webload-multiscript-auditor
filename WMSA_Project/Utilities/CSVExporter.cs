using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMSA.Entities.Interfaces;
using Microsoft.Win32;

namespace WMSA_Project.Utilities
{
    public static class CSVExporter
    {
        public static void ExportScriptCSV(IScript script)
        {
            var saveFileDialoge = new SaveFileDialog()
            {
                Title = "Save CSV File As",
                Filter = "CSV Files (*csv)| *.csv",
            };

            if (saveFileDialoge.ShowDialog() == true)
            {
                try
                {
                    using (var writer = new StreamWriter(saveFileDialoge.FileName, false))
                    {
                        writer.WriteLine("TestGroup,Script,Build,Date,Transaction,Sleep,Verb,URL");

                        foreach (var trans in script.Transactions)
                        {
                            var record = $"{script.TestName},{script.Name},{script.BuildVersion},{script.RecordedDate},{trans.Name},{trans.Sleep}";

                            foreach (var req in trans.Requests)
                            {
                                writer.WriteLine($"{record},{req.Verb.ToString()},{req.URL}");
                            }
                        }
                    }

                }
                catch (System.Exception saveFileException)
                {
                    throw(saveFileException);
                }
            }
        }

        public static void ExportDeltasCSV(System.Data.DataTable table)
        {
            var saveFileDialoge = new SaveFileDialog()
            {
                Title = "Save CSV File As",
                Filter = "CSV Files (*csv)| *.csv",
            };

            if (saveFileDialoge.ShowDialog() == true)
            {
                using (var writer = new StreamWriter(saveFileDialoge.FileName, false))
                {
                    //table.Columns.Count
                } 
            }
        }
    }
}
