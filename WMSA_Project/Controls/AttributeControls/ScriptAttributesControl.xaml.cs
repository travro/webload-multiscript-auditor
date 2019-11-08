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
using System.ComponentModel;
using WMSA_Project.Models;
using WMSA_Project.Windows.AttributeWindows;
using WMSA_Project.Windows.Interfaces;

namespace WMSA_Project.Controls.AttributeControls
{
    /// <summary>
    /// Interaction logic for ScriptAttributeControl1.xaml
    /// </summary>
    public partial class ScriptAttributesControl : UserControl, INotifyPropertyChanged
    {
        private string _selectedValue;
        private IScriptAttributeWindow _scriptAttWindow;

        public ScriptAttributesControl()
        {
            InitializeComponent();
            this.DataContext = this;
            _selectedValue = DefaultValue = "No Selection Made";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public ScriptAttribute Attribute
        {
            get { return (ScriptAttribute)GetValue(AttributeProperty); }
            set { SetValue(AttributeProperty, value); }
        }

        public static readonly DependencyProperty AttributeProperty = DependencyProperty.Register("Attribute", typeof(ScriptAttribute), typeof(ScriptAttributesControl));

        public string DefaultValue { get; }
        public string SelectedValue
        {
            get
            {
                return _selectedValue;
            }
            private set
            {
                _selectedValue = Text_Block.Text = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(_selectedValue));
                }
            }
        }

        public void Clear()
        {
            SelectedValue = DefaultValue;
        }
        
        public bool IsValid()
        {
            return (SelectedValue != null && SelectedValue != "" && SelectedValue != DefaultValue);
        }

        #region handlers
        private void Select_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _scriptAttWindow = new AttributeListWindow(Attribute);
                _scriptAttWindow.ClosedWithAttribute += UpdateSelectedValue;
                _scriptAttWindow.ShowDialog();
            }
            catch (Exception scriptItemException)
            {
                MessageBox.Show(scriptItemException.ToString());
            }
        }
        private void Add_Button_Click(object sender, RoutedEventArgs e)
        {
            _scriptAttWindow = new AttributeAddWindow();
            _scriptAttWindow.ClosedWithAttribute += UpdateSelectedValue;
            _scriptAttWindow.ShowDialog();
        }
        #endregion handlers
        #region helpermethods 
        private void UpdateSelectedValue(object sender, ClosedWithAttributeEventArgs e)
        {
            SelectedValue = e.SelectedValue;
        }
        #endregion
    }
}
