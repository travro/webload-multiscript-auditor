using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
using WMSA_Project.Repositories;
using WMSA_Project.Models;

namespace WMSA_Project.Controls.ContentControls
{
    /// <summary>
    /// Interaction logic for TableViewControl.xaml
    /// </summary>
    public partial class TableViewControl : UserControl
    {
        private DataTable _table;

        public DeltaMode Mode { get; set; }

        public TableViewControl()
        {
            InitializeComponent();

            if (ScriptCollectionContainer.ThisContainer != null)
            {
                ScriptCollectionContainer.ThisContainer.CollectionChanged += UpdateDeltaGrid;
                UpdateDeltaGrid(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset) { });
            }
        }

        public TableViewControl(DeltaMode mode = DeltaMode.ByScript)
        {
            InitializeComponent();
            Mode = mode;
            if (ScriptCollectionContainer.ThisContainer != null)
            {
                ScriptCollectionContainer.ThisContainer.CollectionChanged += UpdateDeltaGrid;
            }
        }
        #region handlers
        private void UpdateDeltaGrid(object sender, NotifyCollectionChangedEventArgs args)
        {
            if (ScriptCollectionContainer.ThisContainer.List.Count > 1)
            {
                _table = BuildDeltaByScriptTable();
                ScrlVwr_Delta.Content = new DataGrid { ItemsSource = _table.DefaultView };
            }
            else
            {
                ScrlVwr_Delta.Content = null;
            }
        }
        #endregion
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

            foreach (var scc in sccList)
            {
                if (sccList.Count == 1) break;
                if (scc == sccList.FirstOrDefault()) continue;

                foreach (var transaction in scc.Container?.Script?.Transactions)
                {
                    var uniqueRequests = transaction.Requests.Where(r => r.Matched != true);

                    if (uniqueRequests != null && uniqueRequests.Count() >= 1)
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

                    if (transaction.RequestsDropped != null && transaction.RequestsDropped.Count() >= 1)
                    {
                        foreach (var req in transaction.RequestsDropped)
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
            return table;
        }
    }
}
