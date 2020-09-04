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
            AddTransBlocks(sccLinkList.First.Value.Container, true);

            if (sccLinkList.Count > 1)
            {
                ConfigNodeRelations(sccLinkList.First.Next);
            }
        }

        #region helpermethods
        private static void ConfigNodeRelations(LinkedListNode<ScriptContainerControl> node)
        {
            if (node == null) return;

            var thisContainer = node.Value.Container;
            var previousContainer = (node.Previous != null) ? node.Previous.Value.Container : null;

            if (previousContainer != thisContainer.PrevComparison)
            {
                thisContainer.PrevComparison = previousContainer;
                thisContainer.Script = ScriptFactory.GetComparativeScriptFromControls(thisContainer);
                AddTransBlocks(thisContainer);
            }
            ConfigNodeRelations(node.Next);
        }
        private static void AddTransBlocks(ScriptControl scriptControl, bool isFirst = false)
        {
            scriptControl.Stack_Transactions.Children.Clear();

            foreach(var t in scriptControl.Script.Transactions)
            {
                var transBlockCtrl = new TransactionBlockControl(t, isFirst);
                transBlockCtrl.ExpanderChanged += scriptControl.OnTransBlockClicked;                
                scriptControl.Stack_Transactions.Children.Add(transBlockCtrl);
            }
        }
        #endregion
        #region handlers
        #endregion
    }
}
