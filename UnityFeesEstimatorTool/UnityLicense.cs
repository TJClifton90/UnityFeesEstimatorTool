namespace TJClifton.UnityFeesEstimatorTool
{
    public class UnityLicense
    {
        public string Name => LicenseType.ToString();

        public readonly UnityLicenseType LicenseType;
        public readonly decimal YearLicenseCostUSD;
        public readonly decimal MonthLicenseCostUSD;

        public readonly uint InstallThresholdForFees;
        public readonly decimal RevenueUSDThresholdForFees;

        public readonly UnityInstallFeeThresholds InstallFeeThresholds;
        public readonly decimal EmergingMarketFeeUSDOverThreshold;

        public UnityLicense(UnityLicenseType licenseType, decimal yearSubscriptionCostUSD, decimal monthSubscriptionCostUSD,
                            uint installThresholdForFees, decimal revenueUSDThresholdForFees, UnityInstallFeeThresholds installFeeThresholds,
                            decimal emergingMarketFeeUSDOverThreshold)
        {
            LicenseType = licenseType;
            YearLicenseCostUSD = yearSubscriptionCostUSD;
            MonthLicenseCostUSD = monthSubscriptionCostUSD;
            InstallThresholdForFees = installThresholdForFees;
            RevenueUSDThresholdForFees = revenueUSDThresholdForFees;
            InstallFeeThresholds = installFeeThresholds;
            EmergingMarketFeeUSDOverThreshold = emergingMarketFeeUSDOverThreshold;
        }

        UnityLicense() : this(UnityLicenseType.Personal, 0, 0, 0, 0, null, 0) { }

        public decimal GetMonthlyLicenseCosts(bool isMonthlyLicense)
        {
            if (isMonthlyLicense)
            {
                return MonthLicenseCostUSD;
            }
            else
            {
                return YearLicenseCostUSD / 12M;
            }
        }
    }
}