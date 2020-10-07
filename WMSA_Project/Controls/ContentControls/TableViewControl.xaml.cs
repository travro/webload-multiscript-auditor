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
using System.Collections.ObjectModel;

namespace WMSA_Project.Controls.ContentControls
{
    /// <summary>
    /// Interaction logic for TableViewControl.xaml
    /// </summary>
    public partial class TableViewControl : UserControl
    {
        public DeltaMode Mode { get; set; }
        public ObservableCollection<ScriptDelta> ScriptDeltas { get; private set; }

        public TableViewControl()
        {
            InitializeComponent();
            DataContext = this;
            ScriptDeltas = new ObservableCollection<ScriptDelta>();

            if (ScriptCollectionContainer.ThisContainer != null)
            {
                ScriptCollectionContainer.ThisContainer.CollectionChanged += UpdateObsCollection;
                UpdateObsCollection(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset) { });
            }
        }

        public TableViewControl(DeltaMode mode = DeltaMode.ByScript)
        {
            InitializeComponent();
            Mode = mode;
            DataContext = this;
            ScriptDeltas = new ObservableCollection<ScriptDelta>();
            if (ScriptCollectionContainer.ThisContainer != null)
            {
                ScriptCollectionContainer.ThisContainer.CollectionChanged += UpdateObsCollection;
                UpdateObsCollection(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset) { });
            }
        }
        #region handlers
        private void UpdateObsCollection(object sender, NotifyCollectionChangedEventArgs args)
        {
            if (ScriptCollectionContainer.ThisContainer.List.Count > 1)
            {
                UpdateCollection(ScriptDeltas);
            }
            else
            {
                ScriptDeltas.Clear();      
            }
        }
        #endregion
        #region helpermethods
        private DataTable BuildDeltaByScriptTable()
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

        private DataTable BuildDeltaByTransactionsTable()
        {
            var table = new DataTable();
            table.Columns.AddRange(new DataColumn[]{
                new DataColumn("Transaction"),
                new DataColumn("From Build"),
                new DataColumn("To Build"),
                new DataColumn("Delta"),
                new DataColumn("Verb"),
                new DataColumn("URL"),
            });

            List<ScriptContainerControl> sccList = ScriptCollectionContainer.ThisContainer.List;

            foreach (var t in sccList[0].Container.Script.Transactions)
            {
                var tIndex = sccList[0].Container.Script.Transactions.IndexOf(t);

                for (int i = 1; i < sccList.Count; i++)
                {
                    var prevScript = sccList[i - 1];
                    var addedReqs = sccList[i].Container.Script.Transactions.First(deltaT => sccList[i].Container.Script.Transactions.IndexOf(deltaT) == tIndex).Requests.Where(req => req.Matched != true);
                    var droppedReqs = sccList[i].Container.Script.Transactions.First(deltaT => sccList[i].Container.Script.Transactions.IndexOf(deltaT) == tIndex).RequestsDropped;

                    foreach (var reqAdded in addedReqs)
                    {
                        var row = table.NewRow();
                        row[0] = t.Name;
                        row[1] = prevScript.Container.Script.BuildVersion;
                        row[2] = sccList[i].Container.Script.BuildVersion;
                        row[3] = "Adds";
                        row[4] = reqAdded.Verb.ToString();
                        row[5] = reqAdded.URL;
                        table.Rows.Add(row);
                    }

                    foreach (var reqDropped in droppedReqs)
                    {
                        var row = table.NewRow();
                        row[0] = t.Name;
                        row[1] = prevScript.Container.Script.BuildVersion;
                        row[2] = sccList[i].Container.Script.BuildVersion;
                        row[3] = "Drops";
                        row[4] = reqDropped.Verb.ToString();
                        row[5] = reqDropped.URL;
                        table.Rows.Add(row);
                    }
                }
            }
            return table;
        }

        private void UpdateCollection(ObservableCollection<ScriptDelta> scriptDeltas)
        {                      
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
                            scriptDeltas.Add(new ScriptDelta(scc.Container.Script.Name, scc.Container.Script.BuildVersion, scc.Container.PrevComparison?.Script.BuildVersion, transaction.Name, "Added", req.Verb.ToString(), req.URL));
                        }
                    }

                    if (transaction.RequestsDropped != null && transaction.RequestsDropped.Count() >= 1)
                    {
                        foreach (var req in transaction.RequestsDropped)
                        {
                            scriptDeltas.Add(new ScriptDelta(scc.Container.Script.Name, scc.Container.Script.BuildVersion, scc.Container.PrevComparison?.Script.BuildVersion, transaction.Name, "Dropped", req.Verb.ToString(), req.URL));
                        }
                    }
                }
            }
        }        
        #endregion
    }

    public struct ScriptDelta
    {
        public string ScriptName { get; set; }
        public string Build { get; set; }
        public string BuildCompared { get; set; }
        public string TransName { get; set; }
        public string ReqDelta { get; set; }
        public string ReqVerb { get; set; }
        public string ReqURL { get; set; }
        public ScriptDelta(string scriptName, string build, string buildCompared, string transName, string delta, string verb, string url)
        {
            ScriptName = scriptName;
            Build = build;
            BuildCompared = buildCompared;
            TransName = transName;
            ReqDelta = delta;
            ReqVerb = verb;
            ReqURL = url;
        }
    }
}
