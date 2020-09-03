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
                //transExpander.Content
                var reqListView = new ListView();
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

                var reqGridView = new GridView();
                reqGridView.Columns.Add(new GridViewColumn() { Header = "Method" });
                reqGridView.Columns.Add(new GridViewColumn() { Header = "URL" });
                reqGridView.Columns.Add(new GridViewColumn() { Header = "Corrs" });
                reqGridView.Columns[0].DisplayMemberBinding = new Binding("Verb");
                reqGridView.Columns[1].DisplayMemberBinding = new Binding("URL");
                reqGridView.Columns[2].DisplayMemberBinding = new Binding("Correlations.Count");
                reqListView.View = reqGridView;                

                if (t.Requests != null)
                {
                    foreach (var r in t.Requests)
                    {
                        //if (r.Correlations != null && r.Correlations.Count > 0)
                        //{
                        //    reqGridView.Columns[2].DisplayMemberBinding = new Binding("Correlations.Count");
                        //}


                        //var reqTxtBlk = new TextBlock();
                        //reqTxtBlk.Text = r.GetInfoString();    
                        //reqTxtBlk.DataContext = reqTxtBlk;

                        //if (!isFirst && !r.Matched)
                        //{
                        //    reqTxtBlk.Foreground = Brushes.Green;
                        //    //reqTxtBlk.Background = Brushes.WhiteSmoke;
                        //    reqTxtBlk.Text = $"+ {reqTxtBlk.Text}";
                        //}

                        //if (r.Correlations != null && r.Correlations.Count > 0)
                        //{
                        //    reqTxtBlk.Text = $"{reqTxtBlk.Text} | {r.Correlations.Count} correlations";
                        //}
                        reqListView.Items.Add(r);
                    }

                    foreach (var dR in t.RequestsDropped)
                    {
                        //var reqTxtBlk = new TextBlock();
                        //reqTxtBlk.Text = dR.GetInfoString();
                        //reqTxtBlk.DataContext = reqTxtBlk;
                        //reqTxtBlk.Foreground = Brushes.Red;
                        //reqTxtBlk.TextDecorations = TextDecorations.Strikethrough;
                        reqListView.Items.Add(dR);
                    }
                }
                transExpander.Content = reqListView;

                /**
                //var reqTree = new TreeView();

                //if (t.Requests != null)
                //{
                //    foreach (var r in t.Requests)
                //    {
                //        var reqTreeViewItem = new TreeViewItem()
                //        {
                //            Header = r.GetInfoString(),
                //        };

                //        reqTreeViewItem.DataContext = reqTreeViewItem;

                //        if (!isFirst && !r.Matched)
                //        {
                //            reqTreeViewItem.Foreground = Brushes.Green;
                //            reqTreeViewItem.Background = Brushes.WhiteSmoke;
                //            reqTreeViewItem.Header = "+" + reqTreeViewItem.Header;
                //        }

                //        if (r.Correlations != null)
                //        {
                //            foreach (var c in r.Correlations)
                //            {
                //                var corrTreeViewItem = new TreeViewItem()
                //                {
                //                    Header = c.GetInfoString(),
                //                };
                //                corrTreeViewItem.DataContext = corrTreeViewItem;
                //                reqTreeViewItem.Items.Add(corrTreeViewItem);
                //            }
                //        }
                //        reqTree.Items.Add(reqTreeViewItem);
                //    }
                //}

                //if (t.RequestsDropped != null && t.RequestsDropped.Count > 0)
                //{
                //    foreach (var unReq in t.RequestsDropped)
                //    {
                //        var unReqTreeViewItem = new TreeViewItem()
                //        {
                //            Header = "-" + unReq.GetInfoString(),
                //            Foreground = Brushes.Red,
                //            Background = Brushes.WhiteSmoke
                //        };
                //        unReqTreeViewItem.DataContext = unReqTreeViewItem;
                //        reqTree.Items.Add(unReqTreeViewItem);
                //    }
                //}

                //transExpander.Content = reqListView;
                */

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
                scriptControl.Stack_Transactions.Children.Add(transExpander);
            }
        }
        #endregion
        #region handlers
        #endregion
    }
}
