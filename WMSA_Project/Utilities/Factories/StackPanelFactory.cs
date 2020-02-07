using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using WMSA_Project.Controls;
using WMSA_Project.Models;
using WMSA_Project.Repositories;
using WMSA_Project.Models.Factories;

namespace WMSA_Project.Utilities.Factories
{
    public static class StackPanelFactory
    {
        public static void BuildStackPanels(LinkedList<ScriptContainerControl> sccLinkList)
        {
            sccLinkList.First.Value.Container.Script.ClearUnmatchedRequests();
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
                var transExpander = new Expander() { Header = new StackPanel() { Orientation = Orientation.Horizontal }};
                transExpander.DataContext = transExpander;

                var reqTree = new TreeView();

                if (t.Requests != null)
                {
                    foreach (var r in t.Requests)
                    {
                        var reqTreeViewItem = new TreeViewItem()
                        {
                            Header = r.GetInfoString(),
                        };

                        reqTreeViewItem.DataContext = reqTreeViewItem;

                        if (!isFirst && !r.Matched)
                        {
                            reqTreeViewItem.Foreground = Brushes.Green;
                            reqTreeViewItem.Header = "+" + reqTreeViewItem.Header;
                        }

                        if (r.Correlations != null)
                        {
                            foreach (var c in r.Correlations)
                            {
                                var corrTreeViewItem = new TreeViewItem()
                                {
                                    Header = c.GetInfoString(),
                                };
                                corrTreeViewItem.DataContext = corrTreeViewItem;
                                reqTreeViewItem.Items.Add(corrTreeViewItem);
                            }
                        }
                        reqTree.Items.Add(reqTreeViewItem);
                    }
                }

                if (t.UnmatchedRequests != null && t.UnmatchedRequests.Count > 0)
                {
                    foreach (var unReq in t.UnmatchedRequests)
                    {
                        var unReqTreeViewItem = new TreeViewItem()
                        {
                            Header = "-" + unReq.GetInfoString(),
                            Foreground = Brushes.Red
                        };
                        reqTree.Items.Add(unReqTreeViewItem);
                    }
                }

                transExpander.Content = reqTree;

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

                if (t.UnmatchedRequests.Count > 0)
                {
                    transExpanderHeader.Children.Add(new TextBlock()
                    {
                        Text = $" (-{t.UnmatchedRequests.Count()})",
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
