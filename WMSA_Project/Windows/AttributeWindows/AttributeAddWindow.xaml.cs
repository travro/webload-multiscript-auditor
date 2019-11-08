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
using WMSA_Project.Windows.Interfaces;

namespace WMSA_Project.Windows.AttributeWindows
{
    /// <summary>
    /// Interaction logic for AttributeAddWindow.xaml
    /// </summary>
    public partial class AttributeAddWindow : Window, IScriptAttributeWindow
    {
        public AttributeAddWindow()
        {
            InitializeComponent();
        }

        public event EventHandler<ClosedWithAttributeEventArgs> ClosedWithAttribute;

        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            ClosedWithAttribute?.Invoke(this, new ClosedWithAttributeEventArgs()
            {
                SelectedValue = Txt_Box.Text
            });
            Close();
        }
    }
}
