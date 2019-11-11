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
using WMSA_Project.Windows;
using WMSA_Project.Utilities;

namespace WMSA_Project.Models.Repositories
{
    public sealed class ScriptRepository : INotifyCollectionChanged
    {
        private LinkedList<ScriptContainerControl> _linkedList;
        private static ScriptRepository _repository;
        private ScriptContainerControl _scriptCntnrCtrl;

        private ScriptRepository()
        {
            /**
             * add new SCC to LL with handlers for buttons
             */
            _linkedList = new LinkedList<ScriptContainerControl>();
            _scriptCntnrCtrl = new ScriptContainerControl(this);

            //add first SCC to linkedlist
            _linkedList.AddFirst(_scriptCntnrCtrl);
            
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public static ScriptRepository Repository
        {
            get
            {
                if(_repository == null)
                {
                    _repository = new ScriptRepository();                     
                }
                return _repository;
            }
            
        }
        public List<ScriptContainerControl> SCCList => _linkedList.ToList();

        #region handlers        
        public void OnContainerScriptAdded(object sender, PropertyChangedEventArgs args)
        {
            OnCollectionChanged();
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
        public bool CanBeAddedToList(Script newScript)
        {
            var scc = _linkedList.First.Value as ScriptContainerControl;

            if (_linkedList.Count >= 1 && scc.Container != null)
            {
                return ScriptTransactionsComparer.CompareAll(scc.Container.Script, newScript);
            }
            else
            {
                return true;
            }
        }
        
        private void OnCollectionChanged()
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
        #endregion

    }
}
