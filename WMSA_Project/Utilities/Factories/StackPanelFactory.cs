using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using WMSA_Project.Controls;
using WMSA_Project.Models;
using WMSA_Project.Models.Repositories;
using WMSA_Project.Models.Factories;

namespace WMSA_Project.Utilities.Factories
{
    public static class StackPanelFactory
    {
        public static void BuildStackPanels(ScriptRepository repo)
        {
            LinkedList<ScriptContainerControl> sccLinkList = new LinkedList<ScriptContainerControl>();

            IEnumerable<ScriptContainerControl> validContainers = repo.ScriptContainerList.Where((sCL) => sCL.Container != null && sCL.Container.Script != null);
            foreach (ScriptContainerControl scc in validContainers)
            {
                sccLinkList.AddLast(scc);
            }
            BuildComparisons(sccLinkList);
        }

        #region helpermethods

        private static void BuildComparisons(LinkedList<ScriptContainerControl> sccLinkList)
        {
            if (sccLinkList.Count == 1)
            {
                sccLinkList.First.Value.Container.Script.ClearUnmatchedRequests();
                BuildPanels(sccLinkList.First.Value.Container);
                return;
            }
            BuildComparativePanels(sccLinkList.First);
        }
        private static void BuildComparativePanels(LinkedListNode<ScriptContainerControl> node)
        {
            if (node == null) return;

            var thisContainer = node.Value.Container;
            var previousContainer = (node.Previous != null)? node.Previous.Value.Container: null;
            var nextContainer = (node.Next != null) ? node.Next.Value.Container : null;

            if (previousContainer != thisContainer.PrevComparison || nextContainer != thisContainer.NextComparison)
            {
                thisContainer.PrevComparison = previousContainer;
                thisContainer.NextComparison = nextContainer;
                thisContainer.Script =  ScriptFactory.GetComparativeScriptFromControls(thisContainer, previousContainer, nextContainer);
                BuildPanels(thisContainer);
            }
            BuildComparativePanels(node.Next);
        }
        private static void BuildPanels(ScriptControl scriptControl)
        {
            scriptControl.Stack_Transactions.Children.Clear();

            foreach (var t in scriptControl.Script.Transactions)
            {
                var transExpander = new Expander()
                {
                    Header = $"{t.Name} ({t.UnmatchedRequests.Count})",
                    Foreground = (t.UnmatchedRequests.Count > 0)? Brushes.Red: Brushes.Black,
                    Content = t.Requests,
                    IsExpanded = false,
                    Background = Brushes.LightGray,
                    FontSize = 12                    
                };

                var reqTree = new TreeView();

                if (t.Requests != null)
                {
                    foreach (var r in t.Requests)
                    {
                        var reqTreeViewItem = new TreeViewItem()
                        {
                            IsExpanded = false,
                            Header = r.GetInfoString(),
                            FontSize = 11
                        };

                        if (r.Correlations != null)
                        {
                            foreach (var c in r.Correlations)
                            {
                                var corrTreeViewItem = new TreeViewItem()
                                {
                                    Header = c.GetInfoString(),
                                    FontSize = 11
                                };
                                reqTreeViewItem.Items.Add(corrTreeViewItem);
                            }
                        }
                        reqTree.Items.Add(reqTreeViewItem);
                    }
                }

                if(t.UnmatchedRequests != null && t.UnmatchedRequests.Count > 0)
                {
                    foreach(var unReq in t.UnmatchedRequests)
                    {
                        var unReqTreeViewItem = new TreeViewItem()
                        {
                            IsExpanded = false,
                            Header = unReq.GetInfoString(),
                            FontSize = 11,
                            Foreground = unReq.SourceColor
                        };
                        reqTree.Items.Add(unReqTreeViewItem);
                    }
                }

                transExpander.Content = reqTree;
                scriptControl.Stack_Transactions.Children.Add(transExpander);
            }
        }
        #endregion
    }
}
