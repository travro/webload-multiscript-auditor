using System;
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
using ScriptFactoryDAL = WMSA_DAL.Service.ScriptFactory;

namespace WMSA_Project.Repositories
{
    public sealed class ScriptRepository : INotifyCollectionChanged
    {
        private static ScriptRepository _repository;
        private LinkedList<ScriptContainerControl> _linkedList;
        private ScriptContainerControl _sccStarter;

        private ScriptRepository()
        {
            _linkedList = new LinkedList<ScriptContainerControl>();
            _sccStarter = new ScriptContainerControl(this);
            _linkedList.AddFirst(_sccStarter);
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
        public void ImportScript(ScriptContainerControl node)
        {
            var importScrptWin = new ImportScriptWindow();
            importScrptWin.ClosedWithScript += (object sender, ClosedWithScriptEventArgs args) =>
            {
                if (args.ScriptOnClose != null && CanAdd(args.ScriptOnClose, node))
                {
                    var scrptCtrl = new ScriptControl(args.ScriptOnClose);
                    scrptCtrl.StackTransExpanderStateChange += OnScriptControlExpanderStateChange;
                    node.Container = scrptCtrl;
                    OnNodeContainerChanged();
                }
                else
                {
                    MessageBox.Show("This script's transactions do not match those of the script(s) currently loaded");
                }
            };
            importScrptWin.ShowDialog();
        }
        public void AddContainerBefore(ScriptContainerControl node)
        {
            _linkedList.AddBefore(_linkedList.Find(node), new ScriptContainerControl(this));
            OnCollectionChanged();
        }
        public void AddContainerAfter(ScriptContainerControl node)
        {
            _linkedList.AddAfter(_linkedList.Find(node), new ScriptContainerControl(this));
            OnCollectionChanged();
        }
        public int GetCount()
        {
            return _linkedList.Count;
        }
        public void Remove(ScriptContainerControl node)
        {
            if (GetCount() > 1)
            {
                if (node.ContainsScript())
                {
                    _linkedList.Remove(_linkedList.Find(node));
                    OnNodeContainerChanged();
                }
                else
                {
                    _linkedList.Remove(_linkedList.Find(node));
                }

                OnCollectionChanged();
            }
            else
            {
                node.Reset();
            }
        }
        public void ExportScript(ScriptContainerControl node)
        {
            //logic for calling DAL service
            ScriptFactoryDAL.SayHi();
        }
        private bool CanAdd(Script newScript, ScriptContainerControl sccCaller)
        {
            if (sccCaller == _sccStarter) return true;
            else if (_linkedList.Count == 1) return true;
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

            foreach(var scc in validLinkList)
            {
                (scc.Container.Stack_Transactions.Children[expanderIndex] as System.Windows.Controls.Expander).IsExpanded = expander.IsExpanded;
            }
        }
        private void OnNodeContainerChanged()
        {
            StackPanelFactory.BuildStackPanels(this);
        }
        private void OnCollectionChanged()
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
        #endregion
    }
}
