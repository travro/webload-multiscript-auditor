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
        private ScriptControl _prevComparison;
        private Script _script;


        private int _totalAdds, _totalDrops;
        public ScriptControl(Script script)
        {
            InitializeComponent();
            Grd_Attributes.Background = ColorDispenser.Dispenser.GetNextColor();
            TxtBlck_Name.Text = $"{script.TestName}";
            TxtBlck_Build.Text = $"{script.BuildVersion}";
            TxtBlck_Script.Text = $"{script.Name}";
            TxtBlck_Date.Text = $"{script.RecordedDate.ToShortDateString()}";
            Script = script;
        }

        public EventHandler<RoutedEventArgs> StackTransExpanderStateChange;

        public Script Script
        {
            get { return _script; }

            set
            {
                _script = value;
                _script.ScriptReset += ClearTotals;
            }
        }
        public ScriptControl PrevComparison
        {
            get { return _prevComparison; }
            set
            {
                _prevComparison = value;
                TxtBlck_PrevComp.Text = $" from {(value as ScriptControl)?.Script.BuildVersion}";
            }
        }
        public int TotalAdds
        {
            set
            {
                _totalAdds = value;
                TxtBlck_TotalAdds.Text = $"(+{_totalAdds.ToString()})";
            }
        }
        public int TotalDrops
        {
            set
            {
                _totalDrops = value;
                TxtBlck_TotalDrops.Text = $"(-{_totalDrops.ToString()})";
            }
        }
        public SolidColorBrush LabelColor { get; set; }
        public void OnStackTransExpanderStateChange(object sender, RoutedEventArgs args)
        {
            StackTransExpanderStateChange?.Invoke(this, args);
        }
        public void ToggleExpander(object sender, RoutedEventArgs args)
        {
            var callingExpndr = (args.Source as Expander);
            int callingExpndrIdx = (sender as ScriptControl).Stack_Transactions.Children.IndexOf(callingExpndr);

            (this.Stack_Transactions.Children[callingExpndrIdx] as Expander).IsExpanded = callingExpndr.IsExpanded;

        }
        private void Btn_AllExpndrs_Click(object sender, RoutedEventArgs e)
        {
            if (Btn_AllExpndrs.Content == "\u02c5")
            {
                foreach (Expander expander in Stack_Transactions.Children)
                {
                    expander.IsExpanded = true;
                }
                Btn_AllExpndrs.Content = "\u02c4";
            }
            else
            {
                foreach (Expander expander in Stack_Transactions.Children)
                {
                    expander.IsExpanded = false;
                }
                Btn_AllExpndrs.Content = "\u02c5";
            }
        }
        private void ClearTotals(object sender, ScriptResetEventArgs args)
        {
            TxtBlck_TotalAdds.Text = TxtBlck_TotalDrops.Text = "";
        }
    }
}
