using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private LinkedList<ScriptContainerControl> _linkedList;
        private static ScriptRepository _repository;
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
        public void OnNodeContainterChanged(object sender, PropertyChangedEventArgs args)
        {
            StackPanelFactory.ProvideStackPanels(this);
        }
        #endregion
        #region helpermethods
        public void AddBefore(ScriptContainerControl node, ScriptContainerControl newNode)
        {
            _linkedList.AddBefore(_linkedList.Find(node), newNode);
            OnCollectionChanged();
        }

        public void AddAfter(ScriptContainerControl node, ScriptContainerControl newNode)
        {
            _linkedList.AddAfter(_linkedList.Find(node), newNode);
            OnCollectionChanged();
        }

        public int GetCount()
        {
            return _linkedList.Count;
        }

        public void Remove(ScriptContainerControl node)
        {
            _linkedList.Remove(_linkedList.Find(node));
            OnCollectionChanged();
        }

        public bool CanAdd(Script newScript, ScriptContainerControl sccCaller)
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

        private void OnCollectionChanged()
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
        #endregion

    }
}
