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
using WMSA_Project.Models;
using WMSA_Project.Models.Factories;
using WMSA_Project.Controls.AttributeControls;
using WMSA_Project.Controls.Interfaces;
using WMSA_Project.DAL.Repositories;

namespace WMSA_Project.Controls.ImportControls
{
    /// <summary>
    /// Interaction logic for DBQueryControl.xaml
    /// </summary>
    public partial class DBQueryControl : UserControl
    {
        public DBQueryControl()
        {
            InitializeComponent();
            DataContext = this;
            SAC_Test.PropertyChanged += CheckScriptSacEnable;
            SAC_Build.PropertyChanged += CheckScriptSacEnable;
            SAC_Script.PropertyChanged += CheckAttributesReady;
            Dt_Pckr.SelectedDateChanged += CheckAttributesReady;

        }

        public event EventHandler<AttributesReadyEventArgs> AttributesReady;

        #region helpermethods
        private void CheckScriptSacEnable(object sender, PropertyChangedEventArgs args)
        {
            if (sender == SAC_Test)
            {
                SAC_Build.Clear();
            }
            if (sender == SAC_Build)
            {
                SAC_Script.Clear();
            }

            if (SAC_Test.IsValid() && SAC_Build.IsValid())
            {
                SAC_Script.Clear();
                AttributesRepository.Repository.BuildScriptCollection(SAC_Test.SelectedValue);
                SAC_Script.IsEnabled = true;
            }
            else
            {
                SAC_Script.IsEnabled = false;
                Dt_Pckr.SelectedDate = null;
            }
        }

        private void CheckAttributesReady(object sender, EventArgs args)
        {
            if (SAC_Test.IsValid() && SAC_Build.IsValid() && SAC_Script.IsValid() && Dt_Pckr.SelectedDate != null)
            {
                OnAttributesReady();
            }
        }

        private void OnAttributesReady()
        {
            if(AttributesReady != null)
            {
                AttributesReady(this, new AttributesReadyEventArgs()
                {
                    SelectedTestName = SAC_Test.SelectedValue,
                    SelectedBuildVersion = SAC_Build.SelectedValue,
                    SelectedScriptName = SAC_Script.SelectedValue,
                    SelectedDate = Dt_Pckr.SelectedDate.Value                
                });
            }
        }
        #endregion
    }

    public class AttributesReadyEventArgs: EventArgs
    {
        public string SelectedTestName { get; set; }
        public string SelectedBuildVersion { get; set; }
        public string SelectedScriptName { get; set; }
        public DateTime SelectedDate { get; set; }
    }
}
