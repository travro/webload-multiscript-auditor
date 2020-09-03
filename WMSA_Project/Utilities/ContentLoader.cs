using System;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMSA_Project.Controls.ContentControls;

namespace WMSA_Project.Utilities
{
    public sealed class ContentLoader
    {
        private ScriptViewControl _scriptViewControl;
        private SutViewControl _sutViewControl;
        private TableViewControl _tableViewControl;

        public ContentLoader() { }

        public UserControl LoadScriptView()
        {
            if (_scriptViewControl == null) { _scriptViewControl = new ScriptViewControl(); }
            return _scriptViewControl;
        }

        public UserControl LoadTableView()
        {
            if (_tableViewControl == null) { _tableViewControl = new TableViewControl(); }
            return _tableViewControl;
        }

        public UserControl LoadSUTView()
        {
            if (_sutViewControl == null) { _sutViewControl = new SutViewControl(); }
            return _sutViewControl;
        }

    }
}
