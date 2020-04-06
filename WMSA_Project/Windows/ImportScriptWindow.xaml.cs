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
using WMSA_Project.Controls.ImportControls;
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
        IScriptImportControl _scrptImprtCtrl;
        Script _scrptFromImprt;

        public ImportScriptWindow()
        {
            InitializeComponent();
            Content_Control.Content = _selectPathCtrl = new SelectScriptPathControl();
        }

        public event EventHandler<ClosedWithScriptEventArgs> ClosedWithScript;

        public Script Script
        {
            get
            { return _scrptFromImprt; }
            private set
            {
                _scrptFromImprt = value;
                _scrptFromImprt.ImportStrategy = _selectPathCtrl.Strategy;
            }
        }

        #region handlers
        private void Btn_Next_Click(object sender, RoutedEventArgs e)
        {

            if (_selectPathCtrl.Strategy == ScriptImportStrategy.FromProjFile)
            {
                _scrptImprtCtrl = new ScriptByFileControl();
            }
            else
            {
                _scrptImprtCtrl = new ScriptByDBControl();
            }
            _scrptImprtCtrl.ScriptReady += ((object ctrlSender, ScriptReadyEventArgs args) => { Btn_Imprt.IsEnabled = true; });
            Content_Control.Content = _scrptImprtCtrl;
            Btn_Imprt.Visibility = Btn_Back.Visibility = Visibility.Visible;
            Btn_Imprt.IsEnabled = false;
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
                Script = _scrptImprtCtrl.GetScript();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                OnClosedWithScript();
            }
        }
        #endregion
        #region helpermethods
        private void OnClosedWithScript()
        {
            ClosedWithScript?.Invoke(this, new ClosedWithScriptEventArgs()
            {
                ScriptOnClose = Script
            });
            Close();
        }
        #endregion
    }

    public class ClosedWithScriptEventArgs : EventArgs
    {
        public Script ScriptOnClose { get; set; }
    }


}
