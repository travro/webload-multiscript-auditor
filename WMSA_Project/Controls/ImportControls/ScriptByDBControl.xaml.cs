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
        private IEnumerable<IScript> _scriptList;

        public event EventHandler<ScriptReadyEventArgs> ScriptReady;


        public ScriptByDBControl()
        {
            InitializeComponent();
            DataContext = this;
            DBQ_Ctrl.SAC_Script.PropertyChanged += UpdateSelectionList;
            Lst_Results.SelectionChanged += Lst_Results_SelectionChanged;
        }
        public Script GetScript()
        {
            throw new NotImplementedException();
        }
        #region handlers
        private void UpdateSelectionList(object sender, PropertyChangedEventArgs args)
        {
            if (DBQ_Ctrl.SAC_Script.SelectedValue != null && DBQ_Ctrl.SAC_Script.SelectedValue != DBQ_Ctrl.SAC_Script.DefaultValue)
            {
                using (var scriptRepo = new ScriptRepo())
                {
                    Lst_Results.ItemsSource = scriptRepo.GetAll()
                        .Where(s =>
                        s.Name == DBQ_Ctrl.SAC_Script.SelectedValue &&
                        s.BuildVersion == DBQ_Ctrl.SAC_Build.SelectedValue
                        ).Select(s => $"{s.Id}, {s.Name}, {s.BuildVersion}, {s.RecordedDate.ToShortDateString()}");
                    //Lst_Results.ItemsSource = _scriptList.Select(s => $"{s.Name}, {s.BuildVersion}, {s.RecordedDate.ToShortDateString()}");
                }
            }
        }

        private void Lst_Results_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ScriptReady?.Invoke(this, new ScriptReadyEventArgs() { Message = "Script is Ready" });
        }
        #endregion
    }
}
