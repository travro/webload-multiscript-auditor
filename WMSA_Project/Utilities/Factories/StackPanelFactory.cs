using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WMSA_Project.Controls;
using WMSA_Project.Models;
using WMSA_Project.Repositories;
using WMSA_Project.Models.Factories;
using System.Windows;
using System.Windows.Data;

namespace WMSA_Project.Utilities.Factories
{
    public static class StackPanelFactory
    {
        public static void BuildStackPanels(LinkedList<ScriptContainerControl> sccLinkList)
        {
            sccLinkList.First.Value.Container.Script.ClearRequestsDropped();
            BuildPanels(sccLinkList.First.Value.Container, true);

            if (sccLinkList.Count > 1)
            {
                BuildComparativePanels(sccLinkList.First.Next);
            }
        }

        #region helpermethods
        private static void BuildComparativePanels(LinkedListNode<ScriptContainerControl> node)
        {
            if (node == null) return;

            var thisContainer = node.Value.Container;
            var previousContainer = (node.Previous != null) ? node.Previous.Value.Container : null;

            if (previousContainer != thisContainer.PrevComparison)
            {
                thisContainer.PrevComparison = previousContainer;
                thisContainer.Script = ScriptFactory.GetComparativeScriptFromControls(thisContainer);
                BuildPanels(thisContainer);
            }
            BuildComparativePanels(node.Next);
        }
        private static void BuildPanels(ScriptControl scriptControl, bool isFirst = false)
        {
            scriptControl.Stack_Transactions.Children.Clear();

            foreach (var t in scriptControl.Script.Transactions)
            {
                var transExpander = new Expander() { Header = new StackPanel() { Orientation = Orientation.Horizontal } };
                transExpander.DataContext = transExpander;

                var reqListView = new ListView();

                //Set MouseWheel event to parent
                reqListView.PreviewMouseWheel += (object sender, MouseWheelEventArgs args) =>
                {
                    if (!args.Handled)
                    {
                        args.Handled = true;
                        var eventArg = new MouseWheelEventArgs(args.MouseDevice, args.Timestamp, args.Delta);
                        eventArg.RoutedEvent = UIElement.MouseWheelEvent;
                        eventArg.Source = sender;
                        var parent = ((Control)sender).Parent as UIElement;
                        parent.RaiseEvent(eventArg);
                    }
                };

                //Format GridView
                var reqGridView = new GridView()
                {

                };
                reqGridView.Columns.Add(new GridViewColumn() { Header = "Method" });
                reqGridView.Columns.Add(new GridViewColumn() { Header = "URL" });
                reqGridView.Columns.Add(new GridViewColumn() { Header = "Corrs" });
                reqGridView.Columns[0].DisplayMemberBinding = new Binding("Verb");
                reqGridView.Columns[1].DisplayMemberBinding = new Binding("URL");
                reqGridView.Columns[2].DisplayMemberBinding = new Binding("Correlations.Count");
                reqListView.View = reqGridView;

                //Add requests to Listview
                if (t.Requests != null)
                {
                    ListViewItem lstViewItem;

                    foreach (var r in t.Requests)
                    {
                        lstViewItem = new ListViewItem()
                        {
                            Content = r,
                            
                        };

                        //assign selection handler
                        lstViewItem.Selected += (object sender, RoutedEventArgs args) => { MessageBox.Show($"Message box clicked, {r.GetInfoString()}"); };

                        if (!isFirst && !r.Matched)
                        {
                            lstViewItem.Foreground = Brushes.Green;
                        }
                        reqListView.Items.Add(lstViewItem);
                    }

                    foreach (var dR in t.RequestsDropped)
                    {

                        dR.URL += " [DROPPED]";

                        lstViewItem = new ListViewItem()
                        {
                            Content = dR,
                            Foreground = Brushes.Red,
                            IsEnabled = false
                        };

                        reqListView.Items.Add(lstViewItem);
                    }
                }
                transExpander.Content = reqListView;

                //Format Transaction Expander Header
                var transExpanderHeader = (transExpander.Header as StackPanel);
                transExpanderHeader.Children.Add(new TextBlock() { Text = t.Name });

                if (t.Sleep != null) transExpanderHeader.Children.Add(new TextBlock() { Text = $" slp:{t.Sleep}" });

                if (!isFirst)
                {
                    int uniqReqNum = t.Requests.Where(r => !r.Matched).Count();

                    if (uniqReqNum > 0)
                    {
                        transExpanderHeader.Children.Add(new TextBlock()
                        {
                            Text = $" (+{uniqReqNum})",
                            Foreground = Brushes.Green
                        });
                    }
                }

                if (t.RequestsDropped.Count > 0)
                {
                    transExpanderHeader.Children.Add(new TextBlock()
                    {
                        Text = $" (-{t.RequestsDropped.Count()})",
                        Foreground = Brushes.Red
                    });
                }

                //Add Transaction to script control stack panel
                scriptControl.Stack_Transactions.Children.Add(transExpander);
            }
        }
        #endregion
        #region handlers
        #endregion
    }
}
