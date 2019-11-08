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
            TxtBlck_TestGroup.Text = script.TestName + " | " + script.BuildVersion;
            TxtBlk_ScriptName.Text = script.Name;
            TxtBlck_ScriptDate.Text = script.RecordedDate.ToShortDateString();

            foreach (var t in script.Transactions)
            {

                var xpndr = new Expander()
                {
                    Header = t.Name,
                    Content = t.Requests,
                    IsExpanded = false,
                    Background = Brushes.LightPink,
                    FontSize = 14
                };

                var reqStack = new StackPanel()
                {
                    Orientation = Orientation.Vertical
                };

                if (t.Requests != null)
                {
                    foreach (var r in t.Requests)
                    {
                        reqStack.Children.Add(new TextBlock()
                        {
                            Text = r.Verb + " " + r.Parameters
                        });
                    }
                }
                xpndr.Content = reqStack;

                Stack_Transactions.Children.Add(xpndr);               

            }
            Script = script;
        }

        public Script Script { get; }
    }
}
