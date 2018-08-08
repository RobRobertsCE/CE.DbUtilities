using System;
using System.Collections.Generic;

namespace CE.PfsConnect.Models
{
    public class PfsDbConnection : PfsConnectSection
    {
        #region properties
        public override string SectionName => PfsConnectSection.DbSectionName;
        public override IList<PfsConnectEntries> EntryKeys { get; set; } = new List<PfsConnectEntries>()
        {
            PfsConnectEntries.DataSource,
            PfsConnectEntries.Catalog,
            PfsConnectEntries.UserID,
            PfsConnectEntries.Password,
            PfsConnectEntries.PasswordEncryption,
            PfsConnectEntries.IntegratedSecurity
        };
        public string DataSource
        {
            get
            {
                return Entries[PfsConnectEntries.DataSource.ToString()];
            }
            set
            {
                Entries[PfsConnectEntries.DataSource.ToString()] = value.ToString();
            }
        }
        public string Catalog
        {
            get
            {
                return Entries[PfsConnectEntries.Catalog.ToString()];
            }
            set
            {
                Entries[PfsConnectEntries.Catalog.ToString()] = value.ToString();
            }
        }
        public string UserID
        {
            get
            {
                return Entries[PfsConnectEntries.UserID.ToString()];
            }
            set
            {
                Entries[PfsConnectEntries.UserID.ToString()] = value.ToString();
            }
        }
        public string Password
        {
            get
            {
                return Entries[PfsConnectEntries.Password.ToString()];
            }
            set
            {
                Entries[PfsConnectEntries.Password.ToString()] = value.ToString();
            }
        }
        public int PasswordEncryption
        {
            get
            {
                return GetInteger(PfsConnectEntries.PasswordEncryption.ToString());
            }
            set
            {
                Entries[PfsConnectEntries.PasswordEncryption.ToString()] = value.ToString();
            }
        }
        public int IntegratedSecurity
        {
            get
            {
                return GetInteger(PfsConnectEntries.IntegratedSecurity.ToString());
            }
            set
            {
                Entries[PfsConnectEntries.IntegratedSecurity.ToString()] = value.ToString();
            }
        }
        public bool IsActive { get; set; }
        public string KeyId
        {
            get
            {
                return $"{DataSource.ToUpper()}:{Catalog.ToUpper()}:{UserID.ToUpper()}:{Password.ToUpper()}";
            }
        }
        #endregion

        #region ctor
        public PfsDbConnection()
            : base()
        {

        }
        #endregion

        #region protected
        internal override void ValidateSection()
        {
            if (IntegratedSecurity < 0 || IntegratedSecurity > 1)
                throw new ArgumentException("IntegratedSecurity must be 0 or 1");

            if (string.IsNullOrEmpty(DataSource))
            {
                throw new ArgumentException($"DataSource cannot be blank");
            }

            if (string.IsNullOrEmpty(Catalog))
            {
                throw new ArgumentException($"Catalog cannot be blank");
            }

            if (IntegratedSecurity == 0)
            {
                if (string.IsNullOrEmpty(UserID))
                {
                    throw new ArgumentException($"UserID cannot be blank");
                }
                if (string.IsNullOrEmpty(Password))
                {
                    throw new ArgumentException($"Password cannot be blank");
                }
            }
        }
        #endregion
    }
}
