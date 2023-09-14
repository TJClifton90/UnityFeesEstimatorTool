namespace UnityFeesEstimatorTool
{
    class UnityInstallFeeThresholds
    {
        public readonly IList<UnityInstallFeeData> Fees;

        public UnityInstallFeeThresholds(IList<UnityInstallFeeData> fees)
        {
            Fees = fees;
        }
    }
}