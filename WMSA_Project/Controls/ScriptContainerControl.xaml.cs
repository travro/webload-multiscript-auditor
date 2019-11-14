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
using WMSA_Project.Models.Repositories;

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
            if (_repo.GetCount() >= 1) { Btn_Exit.Visibility = Visibility.Visible; }
        }

        public ScriptControl Container
        {
            get { return _container; }
            set
            {
                CntCtrl_Main.Content = _container = value; ;
                Btn_Left.Visibility = Btn_Right.Visibility = Btn_Exit.Visibility = Visibility.Visible;
            }
        }

        #region handlers
        private void Btn_Add_Click(object sender, RoutedEventArgs e)
        {
            _repo.AddScriptTo(this);
        }

        private void Btn_Left_Click(object sender, RoutedEventArgs e)
        {
            _repo.AddContainerBefore(this);
        }

        private void Btn_Right_Click(object sender, RoutedEventArgs e)
        {
            _repo.AddContainerAfter(this);
        }

        private void Btn_Exit_Click(object sender, RoutedEventArgs e)
        {
            _repo.Remove(this);
        }
        #endregion

        #region helpermethods
        public void Reset()
        {
            Container = null;
            CntCtrl_Main.ClearValue(ContentProperty);
            Btn_Left.Visibility = Btn_Right.Visibility = Btn_Exit.Visibility = Visibility.Collapsed;
        }
        #endregion
    }
}
