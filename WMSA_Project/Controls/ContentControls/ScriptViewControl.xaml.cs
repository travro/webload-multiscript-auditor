using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
using WMSA_Project.Repositories;

namespace WMSA_Project.Controls.ContentControls
{
    /// <summary>
    /// Interaction logic for ScriptViewControl.xaml
    /// </summary>
    public partial class ScriptViewControl : UserControl
    {
        public ScriptViewControl()
        {
            InitializeComponent();
            DataContext = this;

            foreach (var scc in ScriptCollectionContainer.ThisContainer.List)
            {
                StkPnl_Main.Children.Add(scc);
            }

            if (ScriptCollectionContainer.ThisContainer != null)
            {
                ScriptCollectionContainer.ThisContainer.CollectionChanged += UpdateList;
            }
        }

        private void UpdateList(object sender, NotifyCollectionChangedEventArgs args)
        {
            StkPnl_Main.Children.Clear();

            foreach (var scc in ScriptCollectionContainer.ThisContainer.List)
            {
                StkPnl_Main.Children.Add(scc);
            }
        }
    }
}
