using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WMSA_Project.Models;
using WMSA_Project.Models.Factories;
using WMSA_Project.Repositories;
using WMSA_Project.Controls.Interfaces;
using WMSA_Project.Controls.AttributeControls;
using IScript = WMSA.Entities.Interfaces.IScript;

namespace WMSA_Project.Controls.ImportControls
{
    /// <summary>
    /// Interaction logic for QueryDBControl.xaml
    /// </summary>
    public partial class ScriptByDBControl : UserControl, IScriptImportControl
    {
        public event EventHandler<ScriptReadyEventArgs> ScriptReady;


        public ScriptByDBControl()
        {
            InitializeComponent();
            DataContext = this;
            DBQ_Ctrl.AddButtonsVisible = false;
            DBQ_Ctrl.Dt_Pckr.Visibility = DBQ_Ctrl.Dt_Pckr_TxtBlk.Visibility = Visibility.Hidden;
            DBQ_Ctrl.CollectionChanged += DBQ_Ctrl_CollectionChanged;
            Lst_Results.SelectionChanged += Lst_Results_SelectionChanged;
        }

        public Script GetScript()
        {
            return ScriptFactory.GetScriptFromDB((Lst_Results.SelectedItem as IScript).Id);
        }
        #region handlers

        private void DBQ_Ctrl_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Lst_Results.ItemsSource = DBQ_Ctrl.ScriptList;
        }
        private void Lst_Results_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OnScriptReady();
        }

        private void OnScriptReady()
        {
            ScriptReady?.Invoke(this, new ScriptReadyEventArgs() { Message = "Script is Ready" });
        }
        #endregion
    }
}
