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
using WMSA_Project.Controls;
using WMSA_Project.Models;

namespace WMSA_Project.Controls
{
    /// <summary>
    /// Interaction logic for ScriptContainerControl.xaml
    /// </summary>
    public partial class ScriptContainerControl : UserControl, INotifyPropertyChanged
    {
        private ScriptControl _container;

        public ScriptContainerControl()
        {
            InitializeComponent();
        }

        public event EventHandler<ScriptContainerEventArgs> LeftButtonPressed;
        public event EventHandler<ScriptContainerEventArgs> AddButtonPressed;
        public event EventHandler<ScriptContainerEventArgs> RightButtonPressed;
        public event EventHandler<ScriptContainerEventArgs> ExitButtonPressed;


        public event PropertyChangedEventHandler PropertyChanged;

        public ScriptControl Container
        {
            get
            {
                return _container;
            }
            set
            {
                _container = value;
                CntCtrl_Main.Content = _container;  //<-- uncomment this if autobiding does not work
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Container"));
                Btn_Left.Visibility = Btn_Right.Visibility = Btn_Exit.Visibility = Visibility.Visible;
            }
        }

        #region handlers
        private void Btn_Add_Click(object sender, RoutedEventArgs e)
        {
            AddButtonPressed?.Invoke(this, new ScriptContainerEventArgs(""));
        }

        private void Btn_Left_Click(object sender, RoutedEventArgs e)
        {
            LeftButtonPressed?.Invoke(this, new ScriptContainerEventArgs(""));
        }

        private void Btn_Right_Click(object sender, RoutedEventArgs e)
        {
            RightButtonPressed?.Invoke(this, new ScriptContainerEventArgs(""));
        }

        private void Btn_Exit_Click(object sender, RoutedEventArgs e)
        {
            //raise removal event
            ExitButtonPressed?.Invoke(this, new ScriptContainerEventArgs(""));
        }
        #endregion

        #region helpermethods        
        #endregion
    }

    public class ScriptContainerEventArgs : EventArgs
    {
        public string Value { get; set; }

        public ScriptContainerEventArgs(string args) { Value = args; }
    }
}
