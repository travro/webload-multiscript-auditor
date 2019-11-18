using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using WMSA_Project.Controls;
using WMSA_Project.Models.Repositories;

namespace WMSA_Project.Utilities.Factories
{
    public static class StackPanelFactory
    {
        public static void ProvideStackPanels(ScriptRepository repo)
        {
            LinkedList<ScriptContainerControl> sccLinkList = new LinkedList<ScriptContainerControl>();

            IEnumerable<ScriptContainerControl> validContainers = repo.SCCList.Where((scc) => scc.Container != null && scc.Container.Script !=null);
            foreach(ScriptContainerControl scc in validContainers)
            {
                sccLinkList.AddLast(scc);
            }


            if(sccLinkList.Count == 1)
            {                
                BuildStandardExpanders(sccLinkList.First.Value.Container);
            }
        }

        #region helpermethods
        private static void BuildStandardExpanders(ScriptControl scriptControl)
        {
            foreach (var t in scriptControl.Script.Transactions)
            {
                var transExpander = new Expander()
                {
                    Header = t.Name,
                    Content = t.Requests,
                    IsExpanded = true,
                    Background = Brushes.LightGray,
                    FontSize = 12,
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
                            FontSize = 11,
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
                transExpander.Content = reqTree;
                scriptControl.Stack_Transactions.Children.Add(transExpander);
            }
        }
        
        private static void BuildComparisonExpanders(ScriptControl scriptControl) { }
        #endregion
    }
}
