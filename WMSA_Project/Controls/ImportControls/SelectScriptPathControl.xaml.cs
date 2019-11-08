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

namespace WMSA_Project.Controls.ImportControls
{
    /// <summary>
    /// Interaction logic for SelectScriptPathControl.xaml
    /// </summary>
    public partial class SelectScriptPathControl : UserControl
    {
        public SelectScriptPathControl()
        {
            InitializeComponent();
        }

        public ScriptImportStrategy Strategy
        {
            get { return (ScriptImportStrategy)GetValue(SelectionProperty); }
            set { SetValue(SelectionProperty, value); }
        }

        public static readonly DependencyProperty SelectionProperty = DependencyProperty.Register("ScriptOptionSelected", typeof(ScriptImportStrategy), typeof(SelectScriptPathControl));

        private void RdBtn_File_Checked(object sender, RoutedEventArgs e)
        {
            Strategy = ScriptImportStrategy.FromProjFile;
        }        

        private void RdBtn_DB_Checked(object sender, RoutedEventArgs e)
        {
            Strategy = ScriptImportStrategy.FromDB;
        }
    }

    public enum ScriptImportStrategy
    {
        FromProjFile = 1,
        FromDB = 2
    }
}
