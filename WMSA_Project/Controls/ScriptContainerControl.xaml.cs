using System;
using System.ComponentModel;
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
using WMSA_Project.Controls;
using WMSA_Project.Models;
using WMSA_Project.Models.Repositories;
using WMSA_Project.Windows;

namespace WMSA_Project.Controls
{
    /// <summary>
    /// Interaction logic for ScriptContainerControl.xaml
    /// </summary>
    public partial class ScriptContainerControl : UserControl
    {
        private ScriptControl _container;
        private ScriptRepository _repo;

        public ScriptContainerControl(ScriptRepository repo)
        {
            InitializeComponent();
            _repo = repo;
            //The first Script container control will not show it's exit button, but subsequent controls will
            if (_repo.GetCount() > 1) { Btn_Exit.Visibility = Visibility.Visible; }


        }

        public ScriptControl Container
        {
            get
            {
                return _container;
            }
            set
            {
                _container = value;
                CntCtrl_Main.Content = _container;
                Btn_Left.Visibility = Btn_Right.Visibility = Btn_Exit.Visibility = Visibility.Visible;
            }
        }

        #region handlers
        private void Btn_Add_Click(object sender, RoutedEventArgs e)
        {
            var importScrptWin = new ImportScriptWindow();
            importScrptWin.Closed += CheckScriptOnClose;
            importScrptWin.ShowDialog();
        }

        private void Btn_Left_Click(object sender, RoutedEventArgs e)
        {
            _repo.AddBefore(this, new ScriptContainerControl(_repo));
        }

        private void Btn_Right_Click(object sender, RoutedEventArgs e)
        {
            _repo.AddAfter(this, new ScriptContainerControl(_repo));
        }

        private void Btn_Exit_Click(object sender, RoutedEventArgs e)
        {
            if (_repo.GetCount() > 1)
            {
                _repo.Remove(this);
            }
            else
            {
                Container = null;
                CntCtrl_Main.Content = Btn_Add;
                Btn_Left.Visibility = Btn_Right.Visibility = Btn_Exit.Visibility = Visibility.Hidden;
            }

        }
        #endregion

        #region helpermethods
        private void CheckScriptOnClose(object sender, EventArgs args)
        {
            var script = (sender as ImportScriptWindow).Script;

            if (script != null && _repo.CanBeAddedToList(script))
            {
                Container = new ScriptControl((sender as ImportScriptWindow).Script);
            }
            else
            {
                MessageBox.Show("This script's transactions do not match the transactions of the first script you added");
            }
            #endregion
        }
    }
}
