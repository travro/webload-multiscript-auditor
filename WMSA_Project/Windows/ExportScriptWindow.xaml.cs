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
using System.Windows.Shapes;
using WMSA_Project.Models;

namespace WMSA_Project.Windows
{
    /// <summary>
    /// Interaction logic for ExportScriptWindow.xaml
    /// </summary>
    public partial class ExportScriptWindow : Window
    {
        public ExportScriptWindow()
        {
            InitializeComponent();
        }

        public event EventHandler<RoutedEventArgs> ExportStrategySelected;

        public ScriptExportStrategy ExportStrategy { get; set;}

        private void Btn_Cncl_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Btn_Exprt_Click(object sender, RoutedEventArgs e)
        {
            ExportStrategySelected?.Invoke(this, new RoutedEventArgs());
            Close();
        }

        private void RdBtn_File_Checked(object sender, RoutedEventArgs e)
        {
            ExportStrategy = ScriptExportStrategy.ToCSVFile;
        }

        private void RdBtn_DB_Checked(object sender, RoutedEventArgs e)
        {
            ExportStrategy = ScriptExportStrategy.ToDB;
        }
    }
}
