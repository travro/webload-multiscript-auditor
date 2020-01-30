﻿using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WMSA_Project.Models.Factories;
using WMSA_Project.Models;
using WMSA_Project.Controls;
using WMSA_Project.Utilities;
using WMSA_Project.Utilities.Factories;
using WMSA_Project.Windows;
using WMSA_DAL.Service;

namespace WMSA_Project.Repositories
{
    public sealed class ScriptRepository : INotifyCollectionChanged
    {
        private static ScriptRepository _repository;
        private LinkedList<ScriptContainerControl> _linkedList;

        private ScriptRepository()
        {
            _linkedList = new LinkedList<ScriptContainerControl>();
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public static ScriptRepository Repository
        {
            get
            {
                if (_repository == null)
                {
                    _repository = new ScriptRepository();
                }
                return _repository;
            }
        }
        public List<ScriptContainerControl> ScriptContainerList => _linkedList.ToList();

        #region handlers


        #endregion
        #region helpermethods
        public void ImportScript(ScriptContainerControl callingNode)
        {
            var importScrptWin = new ImportScriptWindow();
            importScrptWin.ClosedWithScript += (object sender, ClosedWithScriptEventArgs args) =>
            {
                if (args.ScriptOnClose != null && CanAdd(args.ScriptOnClose, callingNode))
                {
                    var scrptCtrl = new ScriptControl(args.ScriptOnClose);
                    scrptCtrl.StackTransExpanderStateChange += OnScriptControlExpanderStateChange;

                    if (callingNode.Container != null)
                    {
                        callingNode.Container = scrptCtrl;
                        OnNodeContainerChanged();
                    }
                    else
                    {
                        callingNode.Container = scrptCtrl;
                    }
                }
                else
                {
                    MessageBox.Show("This script's transactions do not match those of the script(s) currently loaded");
                }
            };
            importScrptWin.ShowDialog();
        }
        public void ImportScriptBefore(ScriptContainerControl callingNode)
        {

            var newSCControl = new ScriptContainerControl(this);
            ImportScript(newSCControl);

            if (newSCControl.Container != null)
            {
                _linkedList.AddBefore(_linkedList.Find(callingNode), newSCControl);
                OnCollectionChanged();
            }

        }
        public void ImportScriptAfter(ScriptContainerControl callingNode)
        {
            var newSCControl = new ScriptContainerControl(this);
            ImportScript(newSCControl);

            if (newSCControl.Container != null)
            {
                _linkedList.AddAfter(_linkedList.Find(callingNode), newSCControl);
                OnCollectionChanged();
            }
        }
        public void ImportScriptToEndOfList()
        {
            var newSCControl = new ScriptContainerControl(this);
            ImportScript(newSCControl);

            if (newSCControl.Container != null)
            {
                _linkedList.AddLast(newSCControl);
                OnCollectionChanged();
            }
        }
        public void Remove(ScriptContainerControl node)
        {
            _linkedList.Remove(_linkedList.Find(node));
            OnCollectionChanged();
        }
        public void ExportScript(ScriptContainerControl node)
        {
            if (node.Container != null && node.Container.Script != null)
            {
                try
                {
                    var scriptService = new ScriptService(node.Container.Script);
                    scriptService.SaveScript();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                finally
                {
                    AttributesRepository.Repository.Refresh();
                }
            }
        }
        private bool CanAdd(Script newScript, ScriptContainerControl sccCaller)
        {
            if (_linkedList.Count == 0) { return true; }
            else if (_linkedList.First.Value == sccCaller) { return true; }
            else
            {
                var sccFirst = _linkedList.First(scc => scc.Container != null);

                if (sccFirst != null && sccFirst.Container.Script != null)
                {
                    return ScriptTransactionsComparer.CompareEach(newScript, sccFirst.Container.Script);
                }
                return false;
            }
        }
        private void OnScriptControlExpanderStateChange(object sender, RoutedEventArgs args)
        {
            var expander = (args.Source as System.Windows.Controls.Expander);
            int expanderIndex = (sender as ScriptControl).Stack_Transactions.Children.IndexOf(expander);

            var validLinkList = _linkedList.Where(scc => scc.Container != null);

            foreach (var scc in validLinkList)
            {
                (scc.Container.Stack_Transactions.Children[expanderIndex] as System.Windows.Controls.Expander).IsExpanded = expander.IsExpanded;
            }
        }
        private void OnNodeContainerChanged()
        {
            StackPanelFactory.BuildStackPanels(_linkedList);
        }
        private void OnCollectionChanged()
        {
            if (_linkedList.Count >0)
            {
                StackPanelFactory.BuildStackPanels(_linkedList); 
            }
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
        #endregion
    }
}
