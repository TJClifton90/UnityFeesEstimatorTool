namespace UnityFeesEstimatorTool
{
    static class UnityLicenses
    {
        public static Dictionary<UnityLicenseType, UnityLicense> LicensesByType = new Dictionary<UnityLicenseType, UnityLicense>()
        {
            { UnityLicenseType.Personal, new UnityLicense(UnityLicenseType.Personal, 0, 0, 200_000, 200_000, createPersonalLicenseThresholds(), 0.02M) },
            { UnityLicenseType.Pro, new UnityLicense(UnityLicenseType.Pro, 2_040, 185, 1_000_000, 1_000_000, createProLicenseThresholds(), 0.01M) },
            { UnityLicenseType.Enterprise, new UnityLicense(UnityLicenseType.Enterprise, 3_000, 3_000 / 12M, 1_000_000, 1_000_000, createEnterpriseLicenseThresholds(), 0.005M) },
        };

        static UnityInstallFeeThresholds createPersonalLicenseThresholds()
        {
            return new UnityInstallFeeThresholds(new List<UnityInstallFeeData>()
            {
                new UnityInstallFeeData(1, 100_000, 0.2M),
                new UnityInstallFeeData(100_001, 500_000, 0.2M),
                new UnityInstallFeeData(500_001, 1_000_000, 0.2M),
                new UnityInstallFeeData(1_000_000, uint.MaxValue, 0.2M)
            });
        }

        static UnityInstallFeeThresholds createProLicenseThresholds()
        {
            return new UnityInstallFeeThresholds(new List<UnityInstallFeeData>()
            {
                new UnityInstallFeeData(1, 100_000, 0.15M),
                new UnityInstallFeeData(100_001, 500_000, 0.075M),
                new UnityInstallFeeData(500_001, 1_000_000, 0.03M),
                new UnityInstallFeeData(1_000_000, uint.MaxValue, 0.02M)
            });
        }

        static UnityInstallFeeThresholds createEnterpriseLicenseThresholds()
        {
            return new UnityInstallFeeThresholds(new List<UnityInstallFeeData>()
            {
                new UnityInstallFeeData(1, 100_000, 0.125M),
                new UnityInstallFeeData(100_001, 500_000, 0.06M),
                new UnityInstallFeeData(500_001, 1_000_000, 0.02M),
                new UnityInstallFeeData(1_000_000, uint.MaxValue, 0.01M)
            });
        }
    }
}