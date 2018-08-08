using CE.PfsConnect.Models;
using System;

namespace CE.PfsConnect
{
    class PfsConfiguration : IPfsConfiguration
    {
        private string _pfsConnectFilePath = PfsConnectFile.PfsConnectPath;
        private PfsConnectFile _pfsConnectFile = null;

        protected PfsConnectFile PfsConnect
        {
            get
            {
                if (_pfsConnectFile == null)
                    _pfsConnectFile = PfsConnectFile.LoadPfsConnectFile(_pfsConnectFilePath);

                return _pfsConnectFile;
            }
        }

        public PfsConfiguration()
        {

        }

        public PfsConfiguration(string pfsConnectFilePath)
        {
            _pfsConnectFilePath = pfsConnectFilePath;
        }

        public string GetEntry(PfsConnectEntries key)
        {
            return PfsConnect.GetEntry(key);
        }

        public void SetEntry(PfsConnectEntries key, string value)
        {
            PfsConnect.SetEntry(key, value);
        }

        public bool IsFileValid()
        {
            try
            {
                PfsConnect.ValidateFile();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public override string ToString()
        {
            return PfsConnect.ToString();
        }
    }
}
