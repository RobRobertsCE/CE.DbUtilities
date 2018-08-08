namespace CE.PfsConnect
{
    public static class PfsConfigurationFactory
    {
        public static IPfsConfiguration GetPfsConfiguraiton()
        {
            return new PfsConfiguration();
        }

        public static IPfsConfiguration GetPfsConfiguraiton(string fileName)
        {
            return new PfsConfiguration(fileName);
        }
    }
}
