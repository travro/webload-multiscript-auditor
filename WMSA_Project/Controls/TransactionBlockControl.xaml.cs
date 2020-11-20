using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Interaction logic for TransactionBlockControl.xaml
    /// </summary>
    public partial class TransactionBlockControl : UserControl
    {
        public TransactionBlockControl()
        {
            InitializeComponent();
            BubbleMouseWheelEvent();
        }

        public TransactionBlockControl(Transaction t, bool firstColumn = false)
        {
            InitializeComponent();
            BubbleMouseWheelEvent();
            FormatExpanderHeader(t, firstColumn);
            FormatListView(t, firstColumn);
        }
        public EventHandler<RoutedEventArgs> ExpanderChanged;

        #region handlers
        private void LstVwItm_Selected(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("selected");
        }
        private void OnExpanderChanged(object sender, RoutedEventArgs e)
        {
            ExpanderChanged?.Invoke(this, e);
        }
        #endregion

        #region helpermethods
        private void BubbleMouseWheelEvent()
        {
            LstVw_Reqs.PreviewMouseWheel += (object sender, MouseWheelEventArgs args) =>
            {
                if (!args.Handled)
                {
                    args.Handled = true;
                    var eventArg = new MouseWheelEventArgs(args.MouseDevice, args.Timestamp, args.Delta);
                    eventArg.RoutedEvent = UIElement.MouseWheelEvent;
                    eventArg.Source = sender;
                    var parent = ((Control)sender).Parent as UIElement;
                    parent.RaiseEvent(eventArg);
                }
            };
        }
        private void FormatExpanderHeader(Transaction t, bool firstColumn)
        {
            var transStackPanel = new StackPanel() { Orientation = Orientation.Horizontal };
            transStackPanel.Children.Add(new TextBlock() { Text = t.Name });

            if (t.Sleep != null) transStackPanel.Children.Add(new TextBlock() { Text = $" slp:{t.Sleep}" });

            if (!firstColumn)
            {
                int uniqReqNum = t.Requests.Where(r => !r.Matched).Count();

                if (uniqReqNum > 0)
                {
                    transStackPanel.Children.Add(new TextBlock()
                    {
                        Text = $" (+{uniqReqNum})",
                        Foreground = Brushes.Green
                    });
                }
            }

            if (t.RequestsDropped.Count > 0)
            {
                transStackPanel.Children.Add(new TextBlock()
                {
                    Text = $" (-{t.RequestsDropped.Count()})",
                    Foreground = Brushes.Red
                });
            }
            Expndr_Trans.Header = transStackPanel;
        }
        private void FormatListView(Transaction t, bool firstColumn)
        {
            if (t.Requests != null)
            {
                ListViewItem lstViewItem;

                foreach (var r in t.Requests)
                {
                    lstViewItem = new ListViewItem() { Content = r };

                    lstViewItem.Selected += (object sender, RoutedEventArgs args) =>
                    {
                        var reqDataWindow = new WMSA_Project.Windows.RequestDataWindow(r);
                        reqDataWindow.ShowDialog();
                    };

                    if (!firstColumn && !r.Matched)
                    {
                        lstViewItem.Foreground = Brushes.Green;
                    }

                    LstVw_Reqs.Items.Add(lstViewItem);
                }

                foreach (var dR in t.RequestsDropped)
                {
                    //dR.URL += " [DROPPED]";

                    lstViewItem = new ListViewItem()
                    {
                        Content = dR,
                        Foreground = Brushes.Red,
                        IsEnabled = false
                    };
                    LstVw_Reqs.Items.Add(lstViewItem);
                }
            }
        }
        #endregion
    }
}
