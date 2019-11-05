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
using WMSA_Project.Controls;
using WMSA_Project.Models.Repositories;

namespace WMSA_Project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            foreach (var scc in ScriptRepository.Repository.SCCList)
            {
                StkPnl_Main.Children.Add(scc);
            }

            if (ScriptRepository.Repository != null)
            {
                ScriptRepository.Repository.CollectionChanged += UpdateList;
            }
        }

        #region handlers
        private void UpdateList(object sender, NotifyCollectionChangedEventArgs args)
        {
            StkPnl_Main.Children.Clear();

            foreach (var scc in ScriptRepository.Repository.SCCList)
            {
                StkPnl_Main.Children.Add(scc);
            }
        }
        #endregion
    }
}
