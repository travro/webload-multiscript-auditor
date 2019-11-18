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
using WMSA_Project.Utilities;

namespace WMSA_Project.Controls
{
    /// <summary>
    /// Interaction logic for ScriptControl.xaml
    /// </summary>
    public partial class ScriptControl : UserControl
    {
        private Script _originalScript;
        private Script _currentScript;
        public ScriptControl(Script script)
        {
            InitializeComponent();
            TxtBlck_TestGroup.Text = $"{script.Name} | {script.BuildVersion} | {script.RecordedDate.ToShortDateString()}";
            TxtBlck_TestGroup.Background = LabelColor = ColorDispenser.Dispenser.GetNextColor();
            _originalScript = Script = script;
        }

        public Script Script
        {
            get { return _currentScript; }
            set { _currentScript = value; /*BuildExpanders(value)*/;}
        }

        public SolidColorBrush LabelColor { get; set; }     
        
        #region helpermethods       
        #endregion
    }
}
