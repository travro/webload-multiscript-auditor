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
using System;

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
                ConfigNodeRelations(sccLinkList.First);
                EqualizeBlockHeights(sccLinkList);
            }
        }

        #region helpermethods
        private static void ConfigNodeRelations(LinkedListNode<ScriptContainerControl> node)
        {
            if (node == null) return;

            var thisContainer = node.Value.Container;
            var previousContainer = (node.Previous != null) ? node.Previous.Value.Container : null;

            if (previousContainer != null && previousContainer != thisContainer.PrevComparison)
            {
                thisContainer.PrevComparison = previousContainer;
                thisContainer.Script = ScriptFactory.GetComparativeScriptFromControls(thisContainer);
                AddTransBlocks(thisContainer);
            }
            thisContainer.NextComparison = (node.Next != null) ? node.Next.Value.Container : null;

            ConfigNodeRelations(node.Next);
        }
        private static void AddTransBlocks(ScriptControl scriptControl, bool isFirst = false)
        {
            scriptControl.Stack_Transactions.Children.Clear();

            foreach(var t in scriptControl.Script.Transactions)
            {
                var transBlockCtrl = new TransactionBlockControl(t, isFirst);
                transBlockCtrl.ExpanderChanged += scriptControl.OnExpanderChanged;                
                scriptControl.Stack_Transactions.Children.Add(transBlockCtrl);
            }
        }
        private static void EqualizeBlockHeights(LinkedList<ScriptContainerControl> sccLinkList)
        {
            var firstNode = sccLinkList.First;
            int transCt = firstNode.Value.Container.Stack_Transactions.Children.Count;
            double maxHt;

            for(int i = 0; i < transCt; i++)
            {
                maxHt = GetMaxHeight(firstNode, i, 0.0);
                SetMaxHeight(firstNode, i, maxHt);
            }
        }
        private static void SetMaxHeight(LinkedListNode<ScriptContainerControl> node, int stackIndex, double maxHt)
        {
            if (node == null) return ;
            ((node.Value?.Container?.Stack_Transactions.Children[stackIndex] as TransactionBlockControl).Expndr_Trans.Content as ListView).Height = maxHt;
            if(node.Next != null)
            {
                SetMaxHeight(node.Next, stackIndex, maxHt);
            }
            else
            {
                return;
            }
        }
        private static double GetMaxHeight(LinkedListNode<ScriptContainerControl> node, int stackIndex, double currentHeight)
        {
            if (node == null) return -1;
            double ht = 24.0 + ((node.Value?.Container?.Stack_Transactions.Children[stackIndex] as TransactionBlockControl).Expndr_Trans.Content as ListView).Items.Count * 20.0;

            if (node.Next != null)
            {

                ht = (ht < currentHeight) ? currentHeight : ht;
                return GetMaxHeight(node.Next, stackIndex, ht);
            }
            else
            {
                return ht;
            }                         
        }
        #endregion
        #region handlers
        #endregion
    }
}
