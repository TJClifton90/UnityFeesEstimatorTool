namespace TJClifton.UnityFeesEstimatorTool
{
    public class SimpleUnityFeeEstimator
    {
        public static decimal CalculateUnityAdminFeesInUSD(UnityLicenseType licenseType, uint totalInstalls)
        {
            var licenseData = UnityLicenses.LicensesByType[licenseType];

            if (totalInstalls < licenseData.InstallThresholdForFees)
            {
                return 0;
            }
            else
            {
                var numberOfInstallsOverThreshold = totalInstalls - licenseData.InstallThresholdForFees;

                decimal totalAdminFeesUSD = 0;
                while (numberOfInstallsOverThreshold > 0)
                {
                    for (int i = 0; i < licenseData.InstallFeeThresholds.Fees.Count; ++i)
                    {
                        var thresholdData = licenseData.InstallFeeThresholds.Fees[i];

                        if (numberOfInstallsOverThreshold >= thresholdData.MinimumInstallsOverThreshold)
                        {
                            var numberOfFeesToApply = (Math.Min(numberOfInstallsOverThreshold, thresholdData.MaximumInstallsOverThreshold) - thresholdData.MinimumInstallsOverThreshold) + 1;
                            totalAdminFeesUSD += numberOfFeesToApply * thresholdData.InstallFeeUSD;
                        }
                        else
                        {
                            numberOfInstallsOverThreshold = 0;
                            break;
                        }

                        if (numberOfInstallsOverThreshold < thresholdData.MaximumInstallsOverThreshold)
                        {
                            numberOfInstallsOverThreshold = 0;
                            break;
                        }
                    }
                }

                return totalAdminFeesUSD;
            }
        }
    }
}