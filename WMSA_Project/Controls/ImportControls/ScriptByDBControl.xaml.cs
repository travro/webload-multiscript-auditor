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
using WMSA_Project.Controls.Interfaces;

namespace WMSA_Project.Controls.ImportControls
{
    /// <summary>
    /// Interaction logic for QueryDBControl.xaml
    /// </summary>
    public partial class ScriptByDBControl : UserControl,IScriptImportControl
    {
        public ScriptByDBControl()
        {
            InitializeComponent();
        }

        public event EventHandler<ScriptReadyEventArgs> ScriptReady;

        public Script GetScript()
        {
            throw new NotImplementedException();
        }
    }
}
