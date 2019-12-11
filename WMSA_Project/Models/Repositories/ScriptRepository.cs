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

namespace WMSA_Project.Models.Repositories
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
        public List<ScriptContainerControl> SCCList => _linkedList.ToList();

        #region handlers


        #endregion
        #region helpermethods
        public void AddScriptTo(ScriptContainerControl node)
        {
            var importScrptWin = new ImportScriptWindow();
            importScrptWin.ClosedWithScript += (object sender, ClosedWithScriptEventArgs args) =>
            {
                if (args.ScriptOnClose != null && CanAdd(args.ScriptOnClose, node))
                {
                    node.Container = new ScriptControl(args.ScriptOnClose);
                    OnNodeContainterChanged();
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
                _linkedList.Remove(_linkedList.Find(node));
                OnCollectionChanged();
            }
            else
            {
                node.Reset();
            }
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
                    return ScriptTransactionsComparer.CompareCount(newScript, sccFirst.Container.Script);
                }
                return false;
            }
        }
        private void OnNodeContainterChanged()
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
