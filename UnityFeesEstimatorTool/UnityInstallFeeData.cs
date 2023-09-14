namespace TJClifton.UnityFeesEstimatorTool
{
    public class UnityInstallFeeData
    {
        public uint MinimumInstallsOverThreshold { get; private set; }
        public uint MaximumInstallsOverThreshold { get; private set; }
        public decimal InstallFeeUSD { get; private set; }

        public UnityInstallFeeData(uint minimumInstallsOverThreshold, uint maximumInstallsOverThreshold, decimal installFeeUSD)
        {
            MinimumInstallsOverThreshold = minimumInstallsOverThreshold;
            MaximumInstallsOverThreshold = maximumInstallsOverThreshold;
            InstallFeeUSD = installFeeUSD;
        }
    }
}