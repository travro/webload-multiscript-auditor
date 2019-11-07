using Microsoft.Win32;
using System;
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
using WMSA_Project.Models.ModelFactories;
using WMSA_Project.Controls.Interfaces;

namespace WMSA_Project.Controls
{
    /// <summary>
    /// Interaction logic for SelectFileControl.xaml
    /// </summary>
    public partial class SelectFileControl : UserControl, IScriptImportControl
    {
        public SelectFileControl()
        {
            InitializeComponent();
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

            if (FilePath != null)
            {
                OnScriptReady();
            }
        }
        #endregion
        #region helpermethods
        private void OnScriptReady()
        {
            ScriptReady?.Invoke(this, new ScriptReadyEventArgs() { Message = "Script is Ready" });
        }
        #endregion
    }
}
