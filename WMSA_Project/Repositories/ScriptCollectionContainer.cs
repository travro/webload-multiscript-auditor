using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WMSA_Project.Models;
using WMSA_Project.Controls;
using WMSA_Project.Utilities;
using WMSA_Project.Utilities.Factories;
using WMSA_Project.Windows;

namespace WMSA_Project.Repositories
{
    public sealed class ScriptCollectionContainer : INotifyCollectionChanged
    {
        private static ScriptCollectionContainer _collection;
        private LinkedList<ScriptContainerControl> _linkedList;

        private ScriptCollectionContainer()
        {
            _linkedList = new LinkedList<ScriptContainerControl>();
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public static ScriptCollectionContainer ThisContainer
        {
            get
            {
                if (_collection == null)
                {
                    _collection = new ScriptCollectionContainer();
                }
                return _collection;
            }
        }
        public List<ScriptContainerControl> List => _linkedList.ToList();

        #region handlers


        #endregion
        #region helpermethods
        public void ImportScript(ScriptContainerControl caller)
        {
            var importScrptWin = new ImportScriptWindow();
            importScrptWin.ClosedWithScript += (object sender, ClosedWithScriptEventArgs args) =>
            {
                if (args.ScriptOnClose != null && CanAdd(args.ScriptOnClose, caller))
                {
                    var scrptCtrl = new ScriptControl(args.ScriptOnClose);

                    if (caller.Container != null)
                    {
                        caller.Container = scrptCtrl;
                        OnCollectionChanged();
                    }
                    else
                    {
                        caller.Container = scrptCtrl;
                    }
                }
                else
                {
                    MessageBox.Show("This script's transactions do not match those of the script(s) currently loaded");
                }
            };
            importScrptWin.ShowDialog();
        }
        public void ImportScriptBefore(ScriptContainerControl caller)
        {

            var newSCControl = new ScriptContainerControl(this);
            ImportScript(newSCControl);

            if (newSCControl.Container != null)
            {
                _linkedList.AddBefore(_linkedList.Find(caller), newSCControl);
                OnCollectionChanged();
            }

        }
        public void ImportScriptAfter(ScriptContainerControl caller)
        {
            var newSCControl = new ScriptContainerControl(this);
            ImportScript(newSCControl);

            if (newSCControl.Container != null)
            {
                _linkedList.AddAfter(_linkedList.Find(caller), newSCControl);
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
        public void Remove(ScriptContainerControl caller)
        {
            _linkedList.Remove(_linkedList.Find(caller));
            OnCollectionChanged();
        }
        public void ExportScript(ScriptContainerControl caller)
        {
            try
            {
                SciptMetaRepo.ThisRepo.ExportScript(caller.Container.Script);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private bool CanAdd(Script newScript, ScriptContainerControl caller)
        {
            if (_linkedList.Count == 0) { return true; }
            else if (_linkedList.First.Value == caller && _linkedList.First.Next == null)
            {
                return true;
            }
            else if (_linkedList.First.Value == caller && _linkedList.First.Next != null)
            {
                return ScriptTransactionsComparer.CompareCount(newScript, _linkedList.First.Next.Value.Container.Script);
            }
            else
            {
                return ScriptTransactionsComparer.CompareCount(newScript, _linkedList.First.Value.Container.Script);
            }
        }
        private void OnNodeContainerChanged()
        {
            StackPanelFactory.BuildStackPanels(_linkedList);
        }
        private void OnCollectionChanged()
        {
            if (_linkedList.Count > 0)
            {
                StackPanelFactory.BuildStackPanels(_linkedList);
            }
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
        #endregion
    }
}
