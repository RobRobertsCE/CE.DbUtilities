using System;
using System.Linq;

namespace CE.DbUpgrade.Models
{
    class DatabaseVersionScript : IDatabaseVersionScript, IVersioned
    {
        #region properties
        public Version Version { get; private set; }
        public virtual bool IsCallbackVersion { get { return false; } }
        private string _name = String.Empty;
        public string Name
        {
            get
            {
                return _name;
            }
            private set
            {
                _name = value;
                ParseName(_name);
            }
        }
        public string Target { get; private set; }
        /// <summary>
        /// Creating, Updating, etc...
        /// </summary>
        public string ActionDescription { get; set; }
        public string SqlStatement { get; set; }
        #endregion

        #region ctor
        public DatabaseVersionScript()
        {

        }
        public DatabaseVersionScript(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            Name = name;
        }
        #endregion

        #region protected
        // Format: PaymentPlanLatePayments.18.2.32.sql
        protected virtual void ParseName(string name)
        {
            var nameSections = name.Split('.');

            if (nameSections.Count() != 5)
                throw new ArgumentException($"Database version file name, {name}, is not in the correct format: (xxxx.1.2.3.sql)");

            Target = nameSections[0];

            var major = int.Parse(nameSections[1]);
            var minor = int.Parse(nameSections[2]);
            var dbVersion = int.Parse(nameSections[3]);

            Version = new Version(major, minor, dbVersion);
        }
        #endregion
    }
}
