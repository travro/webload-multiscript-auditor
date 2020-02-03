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
using WMSA_Project.DAL;
using WMSA_Project.Repositories;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace WMSA_Project.Controls.ImportControls
{
    /// <summary>
    /// Interaction logic for DBQueryControl.xaml
    /// </summary>
    public partial class DBQueryControl : UserControl
    {
        string _dbName = "WLScriptsDB";
        bool _dbAvailable;
        public DBQueryControl()
        {
            InitializeComponent();
            DataContext = this;
            SAC_Test.PropertyChanged += SciptMetaRepo.ThisRepo.FilterScriptsByTestName;
            SAC_Test.PropertyChanged += ClearScriptSAC;
            SAC_Build.PropertyChanged += ClearScriptSAC;
            SAC_Script.PropertyChanged += CheckAttributesReady;
            Dt_Pckr.SelectedDateChanged += CheckAttributesReady;
            CheckDBStatus();            
        }

        public event EventHandler<AttributesReadyEventArgs> AttributesReady;

        public bool AddButtonsVisible
        {
            set
            {
                SAC_Test.Btn_Add.Visibility = SAC_Build.Btn_Add.Visibility = SAC_Script.Btn_Add.Visibility = (value) ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        #region helpermethods
        private async void CheckDBStatus()
        {
            _dbAvailable = await Task.Run(() => SqlConnectionManager.CheckDatabaseAvailability());

            SAC_Test.Btn_Select.IsEnabled = (_dbAvailable) ? true : false;
            SAC_Build.Btn_Select.IsEnabled = (_dbAvailable) ? true : false;
            SAC_Script.Btn_Select.IsEnabled = (_dbAvailable) ? true : false;

            if (!_dbAvailable)
            {
                Txt_Block_DBStatus.Text = $"Database: {_dbName} not found. Check configuration file for correct connection string and restart app";
            }
            else
            {
                Txt_Block_DBStatus.Text = $"Database {_dbName} found";
            }

        }
        private void ClearScriptSAC(object sender, PropertyChangedEventArgs args)
        {
            SAC_Script.Clear();
            Dt_Pckr.SelectedDate = null;

            //if (sender == SAC_Test)
            //{
            //    SAC_Build.Clear();
            //}
            //if (sender == SAC_Build)
            //{
            //    SAC_Script.Clear();
            //}

            //if (SAC_Test.IsValid() && SAC_Build.IsValid())
            //{
            //    SAC_Script.Clear();

            //    if (_dbAvailable)
            //    {
            //        SciptMetaRepo.ThisRepo.BuildScriptCollection(SAC_Test.SelectedValue);
            //    }
            //    SAC_Script.IsEnabled = true;
            //}
            //else
            //{
            //    SAC_Script.IsEnabled = false;
            //    Dt_Pckr.SelectedDate = null;
            //}
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
            if (AttributesReady != null)
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

    public class AttributesReadyEventArgs : EventArgs
    {
        public string SelectedTestName { get; set; }
        public string SelectedBuildVersion { get; set; }
        public string SelectedScriptName { get; set; }
        public DateTime SelectedDate { get; set; }
    }
}
