using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMSA_Project.Models.ModelFactories;
using WMSA_Project.Models;
using WMSA_Project.Controls;
using WMSA_Project.Windows;

namespace WMSA_Project.Models.Repositories
{
    public sealed class ScriptRepository : INotifyCollectionChanged
    {
        private LinkedList<ScriptContainerControl> _scriptContainerControls;
        private static ScriptRepository _repository;
        private ScriptContainerControl _scriptCntnrCtrl;

        private ScriptRepository()
        {
            /**
             * add new SCC to LL with handlers for buttons
             */
            _scriptContainerControls = new LinkedList<ScriptContainerControl>();
            _scriptCntnrCtrl = new ScriptContainerControl();
            _scriptCntnrCtrl.AddButtonPressed += SCCAddBtnHandler;
            _scriptCntnrCtrl.PropertyChanged += OnContainerScriptAdded;
            //add first SCC to linkedlist
            _scriptContainerControls.AddFirst(_scriptCntnrCtrl);
            
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
        public List<ScriptContainerControl> SCCList => _scriptContainerControls.ToList();

        #region handlers
        private void SCCAddBtnHandler(object sender, ScriptContainerEventArgs args)
        {
            var importScrptWin = new ImportScriptWindow();
            importScrptWin.Closed += CheckScriptOnClose;
            importScrptWin.Show();
        }

        private void CheckScriptOnClose(object sender, EventArgs args)
        {
            if((sender as ImportScriptWindow).Script != null)
            {
                _scriptCntnrCtrl.Container = new ScriptControl((sender as ImportScriptWindow).Script);
            }
        }
        
        private void OnContainerScriptAdded(object sender, PropertyChangedEventArgs args)
        {
            OnCollectionChanged();
        }

        #endregion
        #region helpermethods
        private void OnCollectionChanged()
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
        #endregion

    }
}
