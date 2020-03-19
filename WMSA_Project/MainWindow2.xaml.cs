﻿using System.Collections.Specialized;
using WMSA_Project.Repositories;
using WMSA_Project.Controls;
using System.Windows;

namespace WMSA_Project
{
    public partial class MainWindow
    {
        private void UpdateDeltaGrid(object sender, NotifyCollectionChangedEventArgs args)
        {
            if (ScriptCollectionContainer.ThisContainer.List.Count > 1) 
            {
                var deltaDatagridCtrl = new DeltaDatagridControl();
                deltaDatagridCtrl.BuildDeltaByScriptTable();

                ScrlVwr_Delta.Content = deltaDatagridCtrl;

            }
            else
            {
                ScrlVwr_Delta.Content = null;
            }
        }

    }
}