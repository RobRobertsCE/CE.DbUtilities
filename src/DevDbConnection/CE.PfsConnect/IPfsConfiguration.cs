namespace CE.PfsConnect
{
    public interface IPfsConfiguration
    {
        string GetEntry(PfsConnectEntries key);
        void SetEntry(PfsConnectEntries key, string value);
        bool IsFileValid();
    }
}