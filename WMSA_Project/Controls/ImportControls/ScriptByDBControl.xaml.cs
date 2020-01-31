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
using WMSA_Project.Controls.Interfaces;
using WMSA_DAL.Repositories;
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
            DBQ_Ctrl.AddButtonsVisible = true;
            DBQ_Ctrl.SAC_Script.PropertyChanged += UpdateSelectionList;
            Lst_Results.SelectionChanged += Lst_Results_SelectionChanged;
        }

        public Script GetScript()
        {
            return ScriptFactory.GetScriptFromDB((Lst_Results.SelectedItem as IScript).Id);
        }
        #region handlers
        private void UpdateSelectionList(object sender, PropertyChangedEventArgs args)
        {
            if (DBQ_Ctrl.SAC_Script.SelectedValue != null && DBQ_Ctrl.SAC_Script.SelectedValue != DBQ_Ctrl.SAC_Script.DefaultValue)
            {
                using (var scriptRepo = new ScriptRepo())
                {
                    ScriptList = scriptRepo.GetAll()
                        .Where(s => s.Name == DBQ_Ctrl.SAC_Script.SelectedValue && s.BuildVersion == DBQ_Ctrl.SAC_Build.SelectedValue);

                    Lst_Results.ItemsSource = ScriptList;
                }
            }
            else
            {
                Lst_Results.ItemsSource = null;
            }
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
