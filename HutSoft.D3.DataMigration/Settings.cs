using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;

namespace HutSoft.D3.DataMigration
{
    public class Settings
    {
        public Settings()
        {
            GetSettingsFromFile();
        }

        private void GetSettingsFromFile()
        {
            FilePath = string.Format("{0}\\Settings.xml", FileUtility.GetAssemblyDirectory());

            if (!File.Exists(FilePath))
                CreateSettingsFile();

            XmlReader reader = XmlReader.Create(FilePath);
            while (reader.Read())
            {
                if (reader.IsStartElement())
                {
                    switch (reader.Name)
                    {
                        case "AgileSQLiteConnectionString":
                            if (reader.Read())
                                AgileSQLiteConnectionString = reader.Value.Trim();
                            break;
                        case "AgileOracleConnectionString":
                            if (reader.Read())
                                AgileOracleConnectionString = reader.Value.Trim();
                            break;
                        case "VaultServer":
                            if (reader.Read())
                                VaultServer = reader.Value.Trim();
                            break;
                        case "VaultInstance":
                            if (reader.Read())
                                VaultInstance = reader.Value.Trim();
                            break;
                        case "VaultUserName":
                            if (reader.Read())
                                VaultUserName = reader.Value.Trim();
                            break;
                        case "VaultPassword":
                            if (reader.Read())
                                VaultPassword = reader.Value.Trim();
                            break;
                        case "LifeCycleDefName":
                            if (reader.Read())
                                LifeCycleDefName = reader.Value.Trim();
                            break;
                        case "WipStateName":
                            if (reader.Read())
                                WipStateName = reader.Value.Trim();
                            break;
                        case "WipStateID":
                            if (reader.Read())
                            {
                                if (!long.TryParse(reader.Value.Trim(), out long value))
                                {
                                    throw new InvalidOperationException(string.Format("Invalid WipStateID in {0}", FilePath));
                                }
                                WipStateID = value;
                            }
                            break;
                        case "ReleasedStateName":
                            if (reader.Read())
                                ReleasedStateName = reader.Value.Trim();
                            break;
                        case "ReleasedStateID":
                            if (reader.Read())
                            {
                                if (!long.TryParse(reader.Value.Trim(), out long value))
                                {
                                    throw new InvalidOperationException(string.Format("Invalid ReleasedStateID in {0}", FilePath));
                                }
                                ReleasedStateID = value;
                            }
                            break;
                        case "DesignsRootPath":
                            if (reader.Read())
                                DesignsRootPath = reader.Value.Trim();
                            break;
                    }
                }
            }
            reader.Close();
            reader = null;
        }

        private void CreateSettingsFile()
        {
            Assembly assembly = GetType().Assembly;
            BinaryReader reader = new BinaryReader(assembly.GetManifestResourceStream("HutSoft.D3.DataMigration.Settings.xml"));
            FileStream stream = new FileStream(FilePath, FileMode.Create);
            BinaryWriter writer = new BinaryWriter(stream);
            try
            {
                byte[] buffer = new byte[64 * 1024];
                int numread = reader.Read(buffer, 0, buffer.Length);

                while (numread > 0)
                {
                    writer.Write(buffer, 0, numread);
                    numread = reader.Read(buffer, 0, buffer.Length);
                }

                writer.Flush();
            }
            finally
            {
                if (stream != null)
                {
                    stream.Dispose();
                }
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        public void Save()
        {
            SaveSettingsToFile();
        }

        private void SaveSettingsToFile()
        {
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings { Encoding = Encoding.UTF8, Indent = true };
            xmlWriterSettings.OmitXmlDeclaration = true;

            XmlWriter writer = XmlWriter.Create(FilePath, xmlWriterSettings);
            writer.WriteStartElement("Settings");

            writer.WriteElementString("AgileSQLiteConnectionString", AgileSQLiteConnectionString);
            writer.WriteElementString("AgileOracleConnectionString", AgileOracleConnectionString);
            writer.WriteElementString("VaultServer", VaultServer);
            writer.WriteElementString("VaultInstance", VaultInstance);
            writer.WriteElementString("VaultUserName", VaultUserName);
            writer.WriteElementString("VaultPassword", VaultPassword);
            writer.WriteElementString("LifeCycleDefName", LifeCycleDefName);
            writer.WriteElementString("WipStateName", WipStateName);
            writer.WriteElementString("WipStateID", WipStateID.ToString());
            writer.WriteElementString("ReleasedStateName", ReleasedStateName);
            writer.WriteElementString("ReleasedStateID", ReleasedStateID.ToString());
            writer.WriteElementString("DesignsRootPath", DesignsRootPath);

            writer.WriteEndElement();
            writer.WriteEndDocument();

            writer.Flush();
            writer.Close();
            writer = null;
        }

        internal string FilePath { get; set; }
        public string AgileSQLiteConnectionString { get; set; }
        public string AgileOracleConnectionString { get; set; }
        public string VaultServer { get; set; }
        public string VaultInstance { get; set; }
        public string VaultUserName { get; set; }
        public string VaultPassword { get; set; }
        public string LifeCycleDefName { get; set; }
        public string WipStateName { get; set; }
        public long WipStateID { get; set; }
        public string ReleasedStateName { get; set; }
        public long ReleasedStateID { get; set; }
        public string DesignsRootPath { get; set; }
    }
}
