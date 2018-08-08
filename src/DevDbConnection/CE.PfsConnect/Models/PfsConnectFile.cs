using System;
using System.IO;
using System.Linq;
using System.Text;

namespace CE.PfsConnect.Models
{
    public class PfsConnectFile
    {
        #region consts
        public const string PfsConnectPath = @"C:\PfsCommon\PfsConnect.ini";
        #endregion

        #region properties
        public InfoSection Info { get; set; }
        public Sql2000Section Sql2000 { get; set; }
        public PfsDbConnection Connection
        {
            get
            {
                return Sql2000.Connections.FirstOrDefault(c => c.IsActive);
            }
        }
        #endregion

        #region ctor
        public PfsConnectFile()
        {
            Info = new InfoSection();
            Sql2000 = new Sql2000Section();
        }
        #endregion

        #region public static
        public static PfsConnectFile LoadPfsConnectFile()
        {
            return LoadPfsConnectFile(PfsConnectPath);
        }
        public static PfsConnectFile LoadPfsConnectFile(string filePath)
        {
            var pfsConnect = new PfsConnectFile();

            try
            {
                var lines = File.ReadAllLines(filePath);

                PfsConnectSection section = pfsConnect.Sql2000;

                foreach (var lineBuffer in lines)
                {
                    var line = lineBuffer.Trim();

                    if (line == pfsConnect.Sql2000.SectionName)
                    {
                        section = pfsConnect.Sql2000;
                    }
                    else if (line == pfsConnect.Info.SectionName)
                    {
                        section.ParseLines();

                        section = pfsConnect.Info;
                    }
                    else
                    {
                        section.SectionLines.Add(line);
                    }
                }

                section.ParseLines();
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("Error loading PfsConnect file: {0}", ex.Message), ex);
            }

            return pfsConnect;
        }
        #endregion

        #region public
        public void SavePfsConnectFile()
        {
            SavePfsConnectFile(PfsConnectPath);
        }
        public void SavePfsConnectFile(string filePath)
        {
            try
            {
                ValidateFile();

                var fileContent = GetPfsFileContent();

                File.WriteAllText(filePath, fileContent);
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("Error saving PfsConnect file: {0}", ex.Message), ex);
            }
        }
        public string GetEntry(PfsConnectEntries key)
        {
            if (Sql2000.EntryKeys.Contains(key))
            {
                return Sql2000.GetEntry(key.ToString());
            }
            else if (Info.EntryKeys.Contains(key))
            {
                return Info.GetEntry(key.ToString());
            }
            else
            {
                throw new ArgumentException($"'{key}' is not a valid Key");
            }
        }
        public void SetEntry(PfsConnectEntries key, string value)
        {
            if (Sql2000.EntryKeys.Contains(key))
            {
                Sql2000.SetEntry(key.ToString(), value);
            }
            else if (Info.EntryKeys.Contains(key))
            {
                Info.SetEntry(key.ToString(), value);
            }
            else
            {
                throw new ArgumentException($"'{key}' is not a valid Key");
            }
        }
        public override string ToString()
        {
            return GetPfsFileContent();
        }
        #endregion

        #region internal
        internal virtual bool ValidateFile()
        {
            Sql2000.ValidateSection();
            Info.ValidateSection();

            return true;
        }
        #endregion

        #region protected
        protected virtual string GetPfsFileContent()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(Sql2000.WriteLines());
            sb.Append(Info.WriteLines());

            return sb.ToString();
        }
        #endregion
    }
}
