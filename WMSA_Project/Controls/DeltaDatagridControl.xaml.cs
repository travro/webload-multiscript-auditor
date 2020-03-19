using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WMSA_Project.Models;
using WMSA_Project.Repositories;


namespace WMSA_Project.Controls
{
    /// <summary>
    /// Interaction logic for DeltaDatagridControl.xaml
    /// </summary>
    public partial class DeltaDatagridControl : UserControl
    {
        private DeltaMode _mode;
        public DeltaDatagridControl(DeltaMode mode = DeltaMode.ByScript)
        {
            _mode = mode;
            InitializeComponent();
        }

        public DeltaMode Mode
        {
            get { return _mode; }
            set
            {
                _mode = value;

            }
        }
        public DataTable BuildDeltaByScriptTable()
        {
            var table = new DataTable();
            table.Columns.AddRange(new DataColumn[]{
                new DataColumn("Script"),
                new DataColumn("Build"),
                new DataColumn("Compared Build"),
                new DataColumn("Transaction"),
                new DataColumn("Delta"),
                new DataColumn("Verb"),
                new DataColumn("URL"),
            });

            List<ScriptContainerControl> sccList = ScriptCollectionContainer.ThisContainer.List;

            foreach(var scc in sccList)
            {
                if (sccList.Count == 1) break;
                if (scc == sccList.FirstOrDefault()) continue;

                foreach(var transaction in scc.Container?.Script?.Transactions)
                {
                    var uniqueRequests = transaction.Requests.Where(r => r.Matched != true);

                    if(uniqueRequests != null && uniqueRequests.Count() >= 1) 
                    {
                        foreach (var req in uniqueRequests)
                        {
                            DataRow row = table.NewRow();
                            row["Script"] = transaction.Script.Name;
                            row["Build"] = transaction.Script.BuildVersion;
                            row["Compared Build"] = scc.Container?.PrevComparison?.Script?.BuildVersion;
                            row["Transaction"] = transaction.Name;
                            row["Delta"] = "Adds";
                            row["Verb"] = req.Verb;
                            row["URL"] = req.URL;
                            table.Rows.Add(row);
                        }
                    }

                    if(transaction.UnmatchedRequests != null && transaction.UnmatchedRequests.Count() >= 1)
                    {
                        foreach (var req in transaction.UnmatchedRequests)
                        {
                            DataRow row = table.NewRow();
                            row["Script"] = transaction.Script.Name;
                            row["Build"] = transaction.Script.BuildVersion;
                            row["Compared Build"] = scc.Container?.PrevComparison?.Script?.BuildVersion;
                            row["Transaction"] = transaction.Name;
                            row["Delta"] = "Drops";
                            row["Verb"] = req.Verb;
                            row["URL"] = req.URL;
                            table.Rows.Add(row);
                        }
                    }
                }
            }
            CntCtrl.Content = new DataGrid() { ItemsSource = table.DefaultView };

            return table;
        }
    }
}
