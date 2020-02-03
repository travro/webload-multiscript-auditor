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
using WMSA_Project.Repositories;

namespace WMSA_Project.Controls.ImportControls
{
    /// <summary>
    /// Interaction logic for SelectFileControl.xaml
    /// </summary>
    public partial class ScriptByFileControl : UserControl, IScriptImportControl
    {
        string _scriptTestName;
        string _scriptBuildVers;
        string _scriptScriptName;
        DateTime _scriptDate;
        bool _attributesReady = false;

        public ScriptByFileControl()
        {
            InitializeComponent();
            DBQ_Ctrl.AddButtonsVisible = true;
            DBQ_Ctrl.AttributesReady += OnAttributesReady;
        }

        public event EventHandler<ScriptReadyEventArgs> ScriptReady;

        public string FilePath { get; set; }
        public bool AttributesReady
        {
            get { return _attributesReady; }
            private set
            {
                _attributesReady = value;
                OnScriptReady();
            }

        }

        public Script GetScript()
        {
            var script = ScriptFactory.GetScriptFromFilePath(FilePath);

            if(_scriptTestName != null && _scriptBuildVers != null && _scriptScriptName != null && _scriptDate != null)
            {
                script.TestName = _scriptTestName;
                script.BuildVersion = _scriptBuildVers;
                script.Name = _scriptScriptName;
                script.RecordedDate = _scriptDate;
            }
            return script;
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
            OnScriptReady();
        }
        #endregion
        #region helpermethods
        private void OnAttributesReady(object sender, AttributesReadyEventArgs args)
        {
            _scriptTestName = args.SelectedTestName;
            _scriptBuildVers = args.SelectedBuildVersion;
            _scriptScriptName = args.SelectedScriptName;
            _scriptDate = args.SelectedDate;

            AttributesReady = true;
        }
        private void OnScriptReady()
        {
            if (FilePath != null && FilePath != "" && AttributesReady)
            {
                ScriptReady?.Invoke(this, new ScriptReadyEventArgs() { Message = "Script is Ready" }); 
            }
        }
        #endregion

    }
}