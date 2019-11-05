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

namespace WMSA_Project.Controls
{
    /// <summary>
    /// Interaction logic for ScriptControl.xaml
    /// </summary>
    public partial class ScriptControl : UserControl
    {
        public ScriptControl(Script script)
        {
            InitializeComponent();

            foreach (var t in script.Transactions)
            {
                Stack_Transactions.Children.Add(new Expander()
                {
                    Header = t.Name,
                    Content = t.Requests,
                    IsExpanded = false,
                    Background = Brushes.LightPink,
                    FontSize = 14
                });
            }
            Script = script;
        }

        public Script Script { get; }
    }
}
