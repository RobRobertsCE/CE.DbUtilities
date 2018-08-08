using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CE.PfsConnect.Models
{
    public class Sql2000Section : PfsConnectSection
    {
        #region fields
        private IList<string> _dbKeyNames = new List<string>();
        private string _dbConnectionMasterKey = PfsConnectEntries.DataSource.ToString();
        private PfsDbConnection _currentDbConnection = null;
        private IDictionary<string, string> _masterEntries = new Dictionary<string, string>();
        #endregion

        #region properties
        public override IList<PfsConnectEntries> EntryKeys { get; set; } = new List<PfsConnectEntries>()
        {
            PfsConnectEntries.WorkStation,
            PfsConnectEntries.PacketSize,
            PfsConnectEntries.PersistSecurity,
            PfsConnectEntries.SqlDependency
        };
        public IList<PfsConnectEntries> DbKeys = new List<PfsConnectEntries>()
        {
            PfsConnectEntries.DataSource,
            PfsConnectEntries.Catalog,
            PfsConnectEntries.UserID,
            PfsConnectEntries.Password,
            PfsConnectEntries.PasswordEncryption,
            PfsConnectEntries.IntegratedSecurity
        };
        public override string SectionName { get { return "[SQL2000]"; } }
        public string Workstation
        {
            get
            {
                return Entries[PfsConnectEntries.SqlDependency.ToString()];
            }
            set
            {
                Entries[PfsConnectEntries.SqlDependency.ToString()] = value.ToString();
            }
        }
        public int PacketSize
        {
            get
            {
                return GetInteger(PfsConnectEntries.PacketSize.ToString());
            }
            set
            {
                Entries[PfsConnectEntries.PacketSize.ToString()] = value.ToString();
            }
        }
        public bool PersistSecurity { get; set; }
        public int SqlDependency
        {
            get
            {
                return GetInteger(PfsConnectEntries.SqlDependency.ToString());
            }
            set
            {
                Entries[PfsConnectEntries.SqlDependency.ToString()] = value.ToString();
            }
        }
        public IList<PfsDbConnection> Connections { get; set; }
        #endregion

        #region ctor
        public Sql2000Section()
            : base()
        {
            Connections = new List<PfsDbConnection>();
            ((List<string>)_dbKeyNames).AddRange(DbKeys.Select(k => k.ToString()));
        }
        #endregion

        #region public
        public void ClearActiveConnections()
        {
            foreach (var item in Connections)
            {
                if (item.IsActive)
                {
                    item.IsActive = false;
                }
            }
        }
        public override void ParseLine(string line)
        {
            if (String.IsNullOrEmpty(line))
            {
                return;
            }

            var isCommented = line.StartsWith(CommentDelimiter);

            if (line.Contains(KvDelimiter))
            {
                var kv = line.Split(KvDelimiter);
                var key = kv[0].Trim();
                var value = kv[1].Trim();
                string targetKey = isCommented ? key.Substring(1) : key;

                if (_dbKeyNames.Contains(targetKey))
                {
                    if (!isCommented)
                        _masterEntries[targetKey] = value;

                    if (targetKey == _dbConnectionMasterKey)
                    {
                        // start a new DbConnection instance
                        _currentDbConnection = new PfsDbConnection();
                        _currentDbConnection.IsActive = !isCommented;
                        foreach (var kvPair in _masterEntries)
                        {
                            _currentDbConnection.SetEntry(kvPair.Key, kvPair.Value);
                        }
                        Connections.Add(_currentDbConnection);
                    }
                    if (_currentDbConnection != null)
                    {
                        _currentDbConnection.SectionLines.Add(line);
                        _currentDbConnection.SetEntry(targetKey, value);
                    }
                }
                else
                {
                    Entries[targetKey] = value;
                }
            }
        }

        public override string WriteLines()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(SectionName);

            foreach (var entry in Entries)
            {
                sb.AppendLine($"{entry.Key}={entry.Value}");
            }
            sb.AppendLine("");

            foreach (var connection in Connections.Where(c => c.IsActive))
            {
                sb.Append(connection.WriteLines(true));
            }

            //foreach (var connection in Connections.Where(c => c.IsActive == false))
            //{
            //    sb.AppendLine(connection.WriteLines(true, true));
            //}

            return sb.ToString();
        }
        #endregion

        #region protected
        internal override void ValidateSection()
        {
            foreach (var key in EntryKeys)
            {
                if (string.IsNullOrEmpty(GetEntry(key.ToString())))
                {
                    throw new ArgumentException($"{key} cannot be blank");
                }
            }

            if (SqlDependency < 0 || SqlDependency > 1)
                throw new ArgumentException("SqlDependency must be 0 or 1");

            var activeConnections = Connections.Where(c => c.IsActive);

            if (activeConnections.Count() != 1)
            {
                throw new ArgumentException($"{activeConnections.Count()} active database sections, must have exactly 1");
            }

            activeConnections.FirstOrDefault().ValidateSection();
        }
        #endregion
    }
}
