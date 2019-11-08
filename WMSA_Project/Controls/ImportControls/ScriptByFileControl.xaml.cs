using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Collections.Generic;
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
using WMSA_Project.Controls.AttributeControls;
using WMSA_Project.Controls.Interfaces;
using WMSA_Project.DAL.Repositories;

namespace WMSA_Project.Controls.ImportControls
{
    /// <summary>
    /// Interaction logic for SelectFileControl.xaml
    /// </summary>
    public partial class ScriptByFileControl : UserControl, IScriptImportControl
    {
        public ScriptByFileControl()
        {
            InitializeComponent();
            SAC_Test.PropertyChanged += CheckSacEnableStatus;
            SAC_Build.PropertyChanged += CheckSacEnableStatus;
            SAC_Script.PropertyChanged += CheckPushStatus;
            Dt_Pckr.SelectedDateChanged += CheckPushStatus;

        }

        public event EventHandler<ScriptReadyEventArgs> ScriptReady;

        public string FilePath { get; set; }

        public Script GetScript()
        {
            return ScriptFactory.GetScriptFromFilePath(FilePath);
        }

        #region handlers
        private void Btn_OFD_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Title = "Select Webload Project File (*.wlp)",
                Filter = "WLoad Project Files (*.wlp)| *.wlp",
                Multiselect = false
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    TxtBlk_FilePath.Text = FilePath = openFileDialog.FileName;
                }
                catch (System.Exception openFileException)
                {
                    MessageBox.Show(openFileException.ToString());
                }
            }
        }
        #endregion
        #region helpermethods


        private void CheckSacEnableStatus(object sender, PropertyChangedEventArgs args)
        {
            if (sender == SAC_Test)
            {
                SAC_Build.Clear();
            }
            if (sender == SAC_Build)
            {
                SAC_Script.Clear();
            }

            if (SacIsValid(SAC_Test) && SacIsValid(SAC_Build))
            {
                SAC_Script.Clear();
                //calls to SQL DB
                AttributesRepository.Repository.BuildScriptCollection(SAC_Test.SelectedValue);
                SAC_Script.IsEnabled = true;
            }
            else
            {
                SAC_Script.IsEnabled = false;
                Dt_Pckr.SelectedDate = null;
            }
        }
        private bool SacIsValid(ScriptAttributesControl control)
        {
            return (control != null && control.SelectedValue != null && control.SelectedValue != control.DefaultValue);
        }

        private void CheckPushStatus(object sender, EventArgs args)
        {
            if (SacIsValid(SAC_Test) && SacIsValid(SAC_Build) && SacIsValid(SAC_Script) && Dt_Pckr.SelectedDate != null && FilePath != null )
            {
                OnScriptReady();
            }
        }

        private void OnScriptReady()
        {
            ScriptReady?.Invoke(this, new ScriptReadyEventArgs() { Message = "Script is Ready" });
        }
        #endregion

    }
}