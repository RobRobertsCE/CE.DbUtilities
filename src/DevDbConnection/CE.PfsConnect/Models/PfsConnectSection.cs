using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CE.PfsConnect.Models
{
    public abstract class PfsConnectSection
    {
        #region consts
        protected const char KvDelimiter = '=';
        protected const string CommentDelimiter = "#";
        protected const string DbSectionName = "DB";
        #endregion

        #region properties
        public abstract IList<PfsConnectEntries> EntryKeys { get; set; }
        public abstract string SectionName { get; }
        public IList<string> SectionLines { get; set; }
        protected IDictionary<string, string> Entries { get; set; }
        #endregion

        #region ctor
        protected PfsConnectSection()
        {
            Entries = new Dictionary<string, string>();
            SectionLines = new List<string>();
            foreach (var key in EntryKeys)
            {
                Entries.Add(key.ToString(), "");
            }
        }
        #endregion

        #region public
        public virtual void ParseLines()
        {
            foreach (var line in SectionLines)
            {
                ParseLine(line);
            }
        }

        public virtual void ParseLine(string line)
        {
            if ((!line.StartsWith(CommentDelimiter) && (line.Contains(KvDelimiter))))
            {
                var kv = line.Split(KvDelimiter);
                Entries[kv[0]] = kv[1].Trim();
            }
        }

        public virtual void SetEntry(string key, string value)
        {
            PfsConnectEntries keyValidator;
            if (!Enum.TryParse(key, out keyValidator))
            {
                throw new ArgumentException($"'{key}' is not a valid Key");
            }
            Entries[key] = value;
        }

        public virtual string GetEntry(string key)
        {
            PfsConnectEntries keyValidator;
            if (!Enum.TryParse(key, out keyValidator))
            {
                throw new ArgumentException($"'{key}' is not a valid Key");
            }
            return Entries[key];
        }

        public virtual string WriteLines()
        {
            return WriteLines(false);
        }
        public virtual string WriteLines(bool suppressSectionName)
        {
            StringBuilder sb = new StringBuilder();

            if (!suppressSectionName)
                sb.AppendLine(SectionName);

            foreach (var entry in Entries)
            {
                sb.AppendLine($"{entry.Key}={entry.Value}");
            }
            sb.AppendLine("");

            return sb.ToString();
        }
        public virtual string WriteLines(bool suppressSectionName, bool commentedOut)
        {
            StringBuilder sb = new StringBuilder();

            if (!suppressSectionName)
                sb.AppendLine(SectionName);

            foreach (var entry in Entries)
            {
                if (commentedOut)
                    sb.Append(CommentDelimiter);

                sb.AppendLine($"{entry.Key}={entry.Value}");
            }

            return sb.ToString();
        }
        #endregion

        #region protected
        protected virtual int GetInteger(string key)
        {
            return int.Parse(Entries[key]);
        }
        protected virtual bool GetBoolean(string key)
        {
            return bool.Parse(Entries[key]);
        }
        internal abstract void ValidateSection();
        #endregion
    }
}
