using System;
using System.ComponentModel;
using System.Windows;
using System.Linq;
using System.Collections.Generic;
using Script = WMSA_Project.Models.Script;
using WMSA_DAL.Service;
using IScript = WMSA.Entities.Interfaces.IScript;

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

        //
        public void FilterScriptsByTestName(object sender, PropertyChangedEventArgs args)
        {
            _testNameFilter = args.PropertyName;
        }
        //SAVE SCRIPT SHOULD PROBABLY RETURN THE ENTITY META DATA FOR THE LIST SO THE SCRIPTS PROPERTY HAS THE CORRECT ID IN THE DB
        public void ExportScript(IScript script)
        {
            var scriptService = new ScriptService(script);

            try
            {
                var exportedScript = scriptService.SaveScript();
                _sessionUploads.Add(exportedScript);
                //Scripts.Concat(singletonList);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
