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
        public IEnumerable<IScript> ScriptList { get; private set; }

        public ScriptByDBControl()
        {
            InitializeComponent();
            DataContext = this;
            DBQ_Ctrl.AddButtonsVisible = false;
            DBQ_Ctrl.SAC_Test.PropertyChanged += UpdateSelectionList;
            DBQ_Ctrl.SAC_Build.PropertyChanged += UpdateSelectionList;
            DBQ_Ctrl.SAC_Script.PropertyChanged += UpdateSelectionList;
            Lst_Results.SelectionChanged += Lst_Results_SelectionChanged;
        }

        public Script GetScript()
        {
            return ScriptFactory.GetScriptFromDB((Lst_Results.SelectedItem as IScript).Id);
        }
        #region handlers

        //need to refactor to use scriptmed
        private void UpdateSelectionList(object sender, PropertyChangedEventArgs args)
        {
            ScriptList = SciptMetaRepo.ThisRepo.Scripts;

            if (DBQ_Ctrl.SAC_Test.SelectedValue != null && DBQ_Ctrl.SAC_Test.SelectedValue != DBQ_Ctrl.SAC_Test.DefaultValue)
                ScriptList = ScriptList.Where(s => s.TestName == DBQ_Ctrl.SAC_Test.SelectedValue);

            if (DBQ_Ctrl.SAC_Build.SelectedValue != null && DBQ_Ctrl.SAC_Build.SelectedValue != DBQ_Ctrl.SAC_Build.DefaultValue)
                ScriptList = ScriptList.Where(s => s.BuildVersion == DBQ_Ctrl.SAC_Build.SelectedValue);

            if (DBQ_Ctrl.SAC_Script.SelectedValue != null && DBQ_Ctrl.SAC_Script.SelectedValue != DBQ_Ctrl.SAC_Script.DefaultValue)
                ScriptList = ScriptList.Where(s => s.Name == DBQ_Ctrl.SAC_Script.SelectedValue);

            Lst_Results.ItemsSource = ScriptList;
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
