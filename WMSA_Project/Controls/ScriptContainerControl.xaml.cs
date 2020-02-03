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
using WMSA_Project.Repositories;

namespace WMSA_Project.Controls
{
    /// <summary>
    /// Interaction logic for ScriptContainerControl.xaml
    /// </summary>
    public partial class ScriptContainerControl : UserControl
    {
        private ScriptControl _container;
        private ScriptCollectionContainer _repo;

        public ScriptContainerControl(ScriptCollectionContainer repo)
        {
            InitializeComponent();
            _repo = repo;
        }

        public ScriptControl Container
        {
            get { return _container; }
            set
            {
                CntCtrl_Main.Content = _container = value;
                Btn_Export.IsEnabled = (_container.Script.ImportStrategy == Models.ScriptImportStrategy.FromDB) ? false : true;
            }
        }

        #region handlers
        private void Btn_Import_Click(object sender, RoutedEventArgs e)
        {
            _repo.ImportScript(this);
        }

        private void Btn_Left_Click(object sender, RoutedEventArgs e)
        {
            _repo.ImportScriptBefore(this);
        }

        private void Btn_Right_Click(object sender, RoutedEventArgs e)
        {
            _repo.ImportScriptAfter(this);
        }

        private void Btn_Exit_Click(object sender, RoutedEventArgs e)
        {
            _repo.Remove(this);
        }

        private void Btn_Export_Click(object sender, RoutedEventArgs e)
        {
            _repo.ExportScript(this);
            Btn_Export.IsEnabled = false;
        }
        #endregion

        #region helpermethods
        public void Reset()
        {
            Container = null;
            CntCtrl_Main.ClearValue(ContentProperty);
        }
        public bool ContainsScript()
        {
            if (Container != null)
            {
                return (Container.Script != null)? true: false;
            }
            else
            {
                return false;
            }
        }
        #endregion


    }
}
