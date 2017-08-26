using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace HutSoft.D3.DataMigration
{
    internal class MyBackgroundWorker : BackgroundWorker
    {
        public MyBackgroundWorker() { }

        public Guid Guid { get; set; }

        public int ThreadId { get; set; }

        public List<String> FileIds { get; set; }

        private LogUtility _logUtility;

        public LogUtility LogUtility
        {
            get { return _logUtility; }
            set { _logUtility = value; }
        }

        public void Log(string logInfo)
        {
            if (_logUtility == null)
                _logUtility = new LogUtility();
            string str = string.Format("Thread {0}: {1}", ThreadId.ToString(), logInfo);
            _logUtility.Log(str);
        }
    }
}
