using System;
using System.IO;
using System.Text;

namespace HutSoft.D3.DataMigration
{
    internal class LogUtility : IDisposable
    {
        private StreamWriter _streamWriter;
        private string _logPath;

        public LogUtility()
        {
            GetNewLog();
            Log("Log Start");
        }

        public string LogPath { get { return _logPath; } }

        private void GetNewLog()
        {
            string directory = FileUtility.GetAssemblyDirectory() + "\\Logs";
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            string file = DateTime.Now.ToFileTimeUtc() + ".log";
            _logPath = string.Format("{0}\\{1}", directory, file);
        }

        public void Log(string logInfo)
        {
            if (!File.Exists(_logPath))
                _streamWriter = new StreamWriter(_logPath);
            else
                _streamWriter = File.AppendText(_logPath);

            _streamWriter.WriteLine(DateTime.Now);
            _streamWriter.WriteLine(logInfo);
            _streamWriter.WriteLine();

            _streamWriter.Close();
        }

        public string Tail()
        {
            string tail = string.Empty;

            using (FileStream fs = File.Open(_logPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                fs.Seek(Math.Max(-1024, -fs.Length), SeekOrigin.End);
                byte[] bytes = new byte[1024];
                fs.Read(bytes, 0, 1024);
                tail = Encoding.Default.GetString(bytes);
            }

            return tail;
        }

        private long _lastTailPosition = 0;

        public string TailFollow()
        {
            string tailFollow = string.Empty;

            using (FileStream fs = File.Open(_logPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                fs.Position = _lastTailPosition;
                long longBytesToRead = fs.Length - fs.Position;
                int numBytesToRead = (int)fs.Length - (int)fs.Position;
                if (numBytesToRead > 0)
                {
                    int numBytesRead = 0;
                    byte[] bytes = new byte[longBytesToRead];
                    while (numBytesToRead > 0)
                    {
                        int n = fs.Read(bytes, numBytesRead, numBytesToRead);
                        if (n == 0)
                            break;

                        numBytesRead += n;
                        numBytesToRead -= n;
                    }
                    tailFollow = Encoding.Default.GetString(bytes);
                    _lastTailPosition = fs.Position;
                }
            }

            return tailFollow;
        }

        public void Dispose()
        {
            _streamWriter.Dispose();
        }
    }
}
