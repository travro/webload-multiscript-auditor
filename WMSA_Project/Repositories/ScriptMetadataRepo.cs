using System;
using System.Windows;
using System.Linq;
using WMSA_DAL.Repositories;
using System.Collections.Generic;

namespace WMSA_Project.Repositories
{
    public sealed class ScriptMetadataRepo
    {
        private static ScriptMetadataRepo repo = null;
        IEnumerable<string> _testNames;
        IEnumerable<string> _testBuilds;
        IEnumerable<string> _scriptNames;
        public string[] TestNames => _testNames.ToArray();
        public string[] TestBuilds => _testBuilds.ToArray();
        public string[] ScriptNames => _scriptNames?.ToArray() ?? new string[0];
        //private  List<object> _scenarioNames;
        //private  List<object> _scenarioDates;
        public static ScriptMetadataRepo ThisRepo
        {
            get
            {
                if (repo == null)
                {
                    repo = new ScriptMetadataRepo();
                    repo.Refresh();
                }
                return repo;
            }
        }
        private ScriptMetadataRepo() { }

        public void BuildScriptCollection(string testName)
        {
            try
            {
                using (var repo = new ScriptRepo())
                {
                    _scriptNames = repo.GetScriptsByTestName(testName).Select(s => s.Name);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Refresh()
        {
            using (var repo = new TestRepo())
            {
                _testNames = repo.GetAll().OrderBy(t => t.test_name).Select(t => t.test_name).Distinct();
                _testBuilds = repo.GetAll().OrderBy(t => t.build_version).Select(t => t.build_version).Distinct();
            }
        }
    }
}
