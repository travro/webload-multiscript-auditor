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
        bool _attributesReady = false;

        public ScriptByFileControl()
        {
            InitializeComponent();
            DBQ_Ctrl.AddButtonsVisible = false;
            //DBQ_Ctrl.AttributesReady += OnAttributesReady;
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
            script.TestName = DBQ_Ctrl.TestValue;
            script.BuildVersion = DBQ_Ctrl.BuildValue;
            script.Name = (DBQ_Ctrl.ScriptNameValue != null)?  DBQ_Ctrl.ScriptNameValue: script.Name;
            script.RecordedDate = DBQ_Ctrl.DateValue;
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
        private void OnScriptReady()
        {
            if (FilePath != null && FilePath != "" /*&& AttributesReady*/)
            {
                ScriptReady?.Invoke(this, new ScriptReadyEventArgs() { Message = "Script is Ready" }); 
            }
        }
        #endregion
    }
}