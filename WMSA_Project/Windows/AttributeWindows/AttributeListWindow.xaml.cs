﻿using System;
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
using WMSA_Project.Windows.Interfaces;
using WMSA_Project.DAL.Repositories;

namespace WMSA_Project.Windows.AttributeWindows
{
    /// <summary>
    /// Interaction logic for AttributeListWindow.xaml
    /// </summary>
    public partial class AttributeListWindow : Window, IScriptAttributeWindow
    {
        public AttributeListWindow(ScriptAttribute attribute)
        {
            InitializeComponent();
            DataContext = this;

            switch (attribute)
            {
                case ScriptAttribute.TestName: SelectableList = AttributesRepository.Repository.TestNames; break;
                case ScriptAttribute.BuildName: SelectableList = AttributesRepository.Repository.TestBuilds; break;
                case ScriptAttribute.ScriptName: SelectableList = AttributesRepository.Repository.ScriptNames; break;
            }
        }
        public event EventHandler<ClosedWithAttributeEventArgs> ClosedWithAttribute;

        public string[] SelectableList { get; }

        private void List_View_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selection = (sender as ListView).SelectedValue.ToString();

            if(ClosedWithAttribute != null && selection != null)
            {
                ClosedWithAttribute(this, new ClosedWithAttributeEventArgs()
                {
                    SelectedValue = selection
                });
            }
            Close();
        }
    }
}