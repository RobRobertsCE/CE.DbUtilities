using System;
using System.Collections.Generic;

namespace CE.PfsConnect.Models
{
    public class InfoSection : PfsConnectSection
    {
        #region properties
        public override IList<PfsConnectEntries> EntryKeys { get; set; } = new List<PfsConnectEntries>()
        {
            PfsConnectEntries.StationNo,
            PfsConnectEntries.ReportPath
        };
        public override string SectionName { get { return "[Info]"; } }
        public int StationNo
        {
            get
            {
                return GetInteger(PfsConnectEntries.StationNo.ToString());
            }
            set
            {
                Entries[PfsConnectEntries.StationNo.ToString()] = value.ToString();
            }
        }
        public string ReportPath
        {
            get
            {
                return Entries[PfsConnectEntries.ReportPath.ToString()];
            }
            set
            {
                Entries[PfsConnectEntries.ReportPath.ToString()] = value.ToString();
            }
        }
        #endregion

        #region ctor
        public InfoSection()
            : base()
        {

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
        }
        #endregion
    }
}
