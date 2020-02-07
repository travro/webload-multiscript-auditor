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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WMSA_Project.Models;
using WMSA_Project.Utilities;

namespace WMSA_Project.Controls
{
    /// <summary>
    /// Interaction logic for ScriptControl.xaml
    /// </summary>
    public partial class ScriptControl : UserControl
    {
        public ScriptControl(Script script)
        {
            InitializeComponent();
            TxtBlck_TestGroup.Text = $"{script.TestName}  {script.BuildVersion}  {script.Name}  {script.RecordedDate.ToShortDateString()}";
            TxtBlck_TestGroup.Background = LabelColor = ColorDispenser.Dispenser.GetNextColor();
            Script = script;
        }

        public EventHandler<RoutedEventArgs> StackTransExpanderStateChange;

        public Script Script { get; set; }
        public ScriptControl PrevComparison { get; set; }
        public SolidColorBrush LabelColor { get; set; }            
        public void OnStackTransExpanderStateChange(object sender, RoutedEventArgs args)
        {
            if(StackTransExpanderStateChange != null)
            {
                StackTransExpanderStateChange(this, args);
            }
        }        
    }
}
