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
using WMSA_Project.Utilities;
using WMSA_Project.Repositories;

namespace WMSA_Project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ContentLoader _loader;
        public MainWindow()
        {
            InitializeComponent();
            _loader = new ContentLoader();
            ScriptCollectionContainer.ThisContainer.CollectionChanged += (object sender, NotifyCollectionChangedEventArgs args) =>
            {
                CntntCtrl_Main.Content = _loader.LoadScriptView();
                Grd_Menu.IsEnabled = (ScriptCollectionContainer.ThisContainer.Count > 1) ? true : false;
            };
        }

        #region handlers
        private void MenuItem_Import_Click(object sender, RoutedEventArgs e)
        {
            ScriptCollectionContainer.ThisContainer.ImportScriptToEndOfList();
        }

        private void MenuItem_Exit_CLick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Btn_ScrptView_Click(object sender, RoutedEventArgs e)
        {
            CntntCtrl_Main.Content = _loader.LoadScriptView();
        }

        private void Btn_TblView_Click(object sender, RoutedEventArgs e)
        {
            CntntCtrl_Main.Content = _loader.LoadTableView();
        }

        private void Btn_SutView_Click(object sender, RoutedEventArgs e)
        {
            CntntCtrl_Main.Content = _loader.LoadSUTView();
        }        
        
        private void Grd_Menu_MouseEnter(object sender, MouseEventArgs e)
        {
            Grd_Menu.Width = Double.NaN;
            Btn_TblView.Visibility = Btn_ScrptView.Visibility = Visibility.Visible;
        }

        private void Grd_Menu_MouseLeave(object sender, MouseEventArgs e)
        {
            Grd_Menu.Width = 16.0;
            Btn_TblView.Visibility = Btn_ScrptView.Visibility = Visibility.Hidden;
        }
        #endregion


    }
}