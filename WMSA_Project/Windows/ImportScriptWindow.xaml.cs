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
using System.Windows.Shapes;
using WMSA_Project.Controls;
using WMSA_Project.Models;
using WMSA_Project.Controls.Interfaces;

namespace WMSA_Project.Windows
{
    /// <summary>
    /// Interaction logic for ImportScriptWindow.xaml
    /// </summary>
    public partial class ImportScriptWindow : Window
    {
        SelectScriptPathControl _selectPathCtrl;  
        IScriptImportControl _scriptImportCtrl;

        public ImportScriptWindow()
        {
            InitializeComponent();
            Content_Control.Content = _selectPathCtrl = new SelectScriptPathControl();
        }

        public Script Script { get; private set; }

        #region handlers
        private void Btn_Next_Click(object sender, RoutedEventArgs e)
        {          
            if (_selectPathCtrl.Strategy == ScriptImportStrategy.FromProjFile)
            {
                _scriptImportCtrl = new SelectFileControl();
                _scriptImportCtrl.ScriptReady += ((object ctrlSender, ScriptReadyEventArgs args) => { Btn_Imprt.IsEnabled = true; });
            }
            else
            {
                _scriptImportCtrl = new QueryDBControl();
                _scriptImportCtrl.ScriptReady += ((object ctrlSender, ScriptReadyEventArgs args) => { Btn_Imprt.IsEnabled = true; });
            }
            Content_Control.Content = _scriptImportCtrl;
            Btn_Imprt.Visibility =  Btn_Back.Visibility = Visibility.Visible;
            Btn_Next.Visibility = Visibility.Collapsed;
        }

        private void Btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Btn_Back_Click(object sender, RoutedEventArgs e)
        {
            Content_Control.Content = _selectPathCtrl;
            Btn_Imprt.Visibility = Btn_Back.Visibility = Visibility.Collapsed;
            Btn_Next.Visibility = Visibility.Visible;
            Btn_Next.IsEnabled = true;
        }
        private void Btn_Imprt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Script = _scriptImportCtrl.GetScript();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Close();
            }
        }
        #endregion

        #region helpermethods
        #endregion


    }
}
