using System;
using System.ComponentModel;
using System.Windows;
using System.Linq;
using System.Collections.Generic;
using WMSA_DAL.Service;
using IScript = WMSA.Entities.Interfaces.IScript;
using WMSA_Project.Windows;
using Microsoft.Win32;

namespace WMSA_Project.Repositories
{
    public sealed class SciptMetaRepo
    {
        static SciptMetaRepo _repo = null;
        string _testNameFilter = "";
        private IEnumerable<IScript> _scripts;
        private List<IScript> _sessionUploads = new List<IScript>();

        public static SciptMetaRepo ThisRepo
        {
            get
            {
                if (_repo == null)
                {
                    _repo = new SciptMetaRepo();
                }
                return _repo;
            }
        }
        public IEnumerable<IScript> Scripts { get => _scripts.Concat(_sessionUploads); private set => _scripts = value; }
        public IEnumerable<string> ScriptTestGroups => Scripts.OrderBy(s => s.TestName).Select(s => s.TestName).Distinct().ToArray();
        public IEnumerable<string> ScriptTestBuilds => Scripts.OrderBy(s => s.BuildVersion).Select(s => s.BuildVersion).Distinct().ToArray();
        public IEnumerable<string> ScriptNames
        {
            get
            {
                return (_testNameFilter != null && _testNameFilter != "") ?
                    Scripts.Where(s => s.TestName == _testNameFilter).OrderBy(s => s.Name).Select(s => s.Name).Distinct().ToArray() ?? new string[0] :
                    Scripts.OrderBy(s => s.Name).Select(s => s.Name).Distinct().ToArray() ?? new string[0];
            }
        }

        private SciptMetaRepo()
        {
            try
            {
                Scripts = ScriptService.GetTestAndScripts();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void FilterScriptsByTestName(object sender, PropertyChangedEventArgs args)
        {
            _testNameFilter = args.PropertyName;
        }
        public void ExportScript(IScript script)
        {
            try
            {
                var exprtScrptWin = new ExportScriptWindow();
                exprtScrptWin.ExportStrategySelected += (object sender, RoutedEventArgs ars) =>
                {
                    var xsw = (sender as ExportScriptWindow);
                    if (xsw.ExportStrategy == Models.ScriptExportStrategy.ToCSVFile)
                    {
                        var saveFileDialoge = new SaveFileDialog()
                        {
                            Title = "Save CSV File As",
                            Filter = "CSV Files (*csv)| *.csv",                            
                        };
                        

                        if(saveFileDialoge.ShowDialog() == true)
                        {
                            try
                            {
                                WMSA_Project.Utilities.CSVExporter.ExportScriptCSV(script, saveFileDialoge.FileName);
                            }
                            catch(System.Exception saveFileException)
                            {
                                MessageBox.Show(saveFileException.ToString());
                            }
                        }
                    }
                    else
                    {
                        var scriptService = new ScriptService(script);
                        var exportedScript = scriptService.SaveScript();
                        _sessionUploads.Add(exportedScript);
                    }
                };
                exprtScrptWin.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
