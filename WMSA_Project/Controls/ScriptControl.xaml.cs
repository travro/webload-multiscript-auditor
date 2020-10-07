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
            Brdr_Meta.BorderBrush = ColorDispenser.Dispenser.GetNextColor();
            TxtBlck_Name.Text = $"{script.TestName}";
            TxtBlck_Build.Text = $"{script.BuildVersion}";
            TxtBlck_Script.Text = $"{script.Name}";
            TxtBlck_Date.Text = $"{script.RecordedDate.ToShortDateString()}";
            Script = script;
        }

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
        public ScriptControl NextComparison { get; set; }
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

        #region handlers
        public void OnExpanderChanged(object sender, RoutedEventArgs args)
        {
            int tBlockIndx = Stack_Transactions.Children.IndexOf((sender as TransactionBlockControl));
            if (PrevComparison != null)
            {
                PrevComparison.ReceiveMessage(this, tBlockIndx);
            }
            if (NextComparison != null)
            {
                NextComparison.ReceiveMessage(this, tBlockIndx);
            }
        }
        private void Btn_AllExpndrs_Click(object sender, RoutedEventArgs e)
        {
            if (Btn_AllExpndrs.Content == "\u02c5\u02c5\u02c5")
            {
                foreach (TransactionBlockControl transBlock in Stack_Transactions.Children)
                {
                    transBlock.Expndr_Trans.IsExpanded = true;
                }
                Btn_AllExpndrs.Content = "\u02c4\u02c4\u02c4";
            }
            else
            {
                foreach (TransactionBlockControl transBlock in Stack_Transactions.Children)
                {
                    transBlock.Expndr_Trans.IsExpanded = false;
                }
                Btn_AllExpndrs.Content = "\u02c5\u02c5\u02c5";
            }
        }
        private void ClearTotals(object sender, ScriptResetEventArgs args)
        {
            TxtBlck_TotalAdds.Text = TxtBlck_TotalDrops.Text = "";
        }
        #endregion
        #region helper methods
        public void ReceiveMessage(ScriptControl sender, int index)
        {
            if ((this.Stack_Transactions.Children[index] as TransactionBlockControl).Expndr_Trans.IsExpanded != (sender.Stack_Transactions.Children[index] as TransactionBlockControl).Expndr_Trans.IsExpanded)
            {
                (this.Stack_Transactions.Children[index] as TransactionBlockControl).Expndr_Trans.IsExpanded = (sender.Stack_Transactions.Children[index] as TransactionBlockControl).Expndr_Trans.IsExpanded;
            }
            else return;
        }
        #endregion
    }
}
