﻿using System;
using System.ComponentModel;
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
using WMSA_Project.Models;
using WMSA_Project.Models.Factories;
using WMSA_Project.Controls.AttributeControls;
using WMSA_Project.Controls.Interfaces;
using WMSA_Project.DAL;
using WMSA_Project.Repositories;
using IScript = WMSA.Entities.Interfaces.IScript;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace WMSA_Project.Controls.ImportControls
{
    /// <summary>
    /// Interaction logic for DBQueryControl.xaml
    /// </summary>
    public partial class DBQueryControl : UserControl, INotifyCollectionChanged
    {
        string _dbName = "WLScriptsDB";
        bool _dbAvailable;
        public DBQueryControl()
        {
            InitializeComponent();
            DataContext = this;
            SAC_Test.PropertyChanged += ClearScriptandDate;
            SAC_Build.PropertyChanged += ClearScriptandDate;
            SAC_Script.PropertyChanged += CheckAttributesReady;
            Dt_Pckr.SelectedDateChanged += CheckAttributesReady;
            CheckDBStatus();
        }

        public event EventHandler<AttributesReadyEventArgs> AttributesReady;
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public IEnumerable<IScript> ScriptList { get; private set; }
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

            SAC_Test.Btn_Select.IsEnabled = SAC_Build.Btn_Select.IsEnabled = SAC_Script.Btn_Select.IsEnabled = (_dbAvailable) ? true : false;

            if (!_dbAvailable)
            {
                Txt_Block_DBStatus.Text = $"Database: {_dbName} not found. Check configuration file for correct connection string and restart app";
            }
            else
            {
                Txt_Block_DBStatus.Text = $"Database {_dbName} found";
                SAC_Test.PropertyChanged += SciptMetaRepo.ThisRepo.FilterScriptsByTestName;
                SAC_Test.PropertyChanged += UpdateScriptList;
                SAC_Build.PropertyChanged += UpdateScriptList;
                SAC_Script.PropertyChanged += UpdateScriptList;
            }
        }
        private void ClearScriptandDate(object sender, PropertyChangedEventArgs args)
        {
            SAC_Script.Clear();
            Dt_Pckr.SelectedDate = null;
        }
        private void UpdateScriptList(object sender, PropertyChangedEventArgs args)
        {
            ScriptList = SciptMetaRepo.ThisRepo.Scripts;

            if (SAC_Test.IsValid()) ScriptList = ScriptList.Where(s => s.TestName.Contains(SAC_Test.SelectedValue));
            if (SAC_Build.IsValid()) ScriptList = ScriptList.Where(s => s.BuildVersion.Contains(SAC_Build.SelectedValue));
            if (SAC_Script.IsValid()) ScriptList = ScriptList.Where(s => s.Name.Contains(SAC_Script.SelectedValue));
            OnCollectionChanged();
        }
        private void CheckAttributesReady(object sender, EventArgs args)
        {
            if (SAC_Test.IsValid() && SAC_Build.IsValid() && SAC_Script.IsValid() && Dt_Pckr.SelectedDate != null)
            {
                OnAttributesReady();
            }
        }
        private void OnCollectionChanged()
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
        
        private void OnAttributesReady()
        {
            AttributesReady?.Invoke(this, new AttributesReadyEventArgs()
            {
                SelectedTestName = SAC_Test.SelectedValue,
                SelectedBuildVersion = SAC_Build.SelectedValue,
                SelectedScriptName = SAC_Script.SelectedValue,
                SelectedDate = Dt_Pckr.SelectedDate.Value
            });
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
