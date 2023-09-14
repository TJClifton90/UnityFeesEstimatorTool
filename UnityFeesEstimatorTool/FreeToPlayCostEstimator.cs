namespace UnityFeesEstimatorTool
{
    class FreeToPlayCostEstimator
    {
        public readonly IList<FreeToPlayUnityEstimatorMonthlyData> MonthlyData;

        public FreeToPlayCostEstimator(IList<FreeToPlayUnityEstimatorMonthlyData> monthlyData)
        {
            MonthlyData = monthlyData;
        }

        public IList<FreeToPlayEstimatesResult> Calculate(UnityLicenseType licenseType)
        {
            var license = UnityLicenses.LicensesByType[licenseType];

            uint totalNumberOfInstalls = 0;
            uint installsWithFeePaid = 0;
            uint installsWithoutFeePaid = 0;
            decimal totalGrossRevenueUSD = 0;

            var costResults = new List<FreeToPlayEstimatesResult>();

            for (int i = 0; i < MonthlyData.Count; i++)
            {
                var averageRevenuePerInstallUSD = MonthlyData[i].LTV - MonthlyData[i].CPI;
                var monthlyGrossRevenue = averageRevenuePerInstallUSD * MonthlyData[i].NumberOfInstalls;
                totalGrossRevenueUSD += monthlyGrossRevenue;

                totalNumberOfInstalls += MonthlyData[i].NumberOfInstalls;

                if (HasFeeThresholdReachedBeenReached(license, totalNumberOfInstalls, totalGrossRevenueUSD))
                {
                    if (installsWithFeePaid < license.InstallThresholdForFees)
                    {
                        installsWithFeePaid = license.InstallThresholdForFees;
                    }
                }
                else
                {
                    installsWithFeePaid += MonthlyData[i].NumberOfInstalls;
                    if (installsWithFeePaid > license.InstallThresholdForFees)
                    {
                        installsWithFeePaid = license.InstallThresholdForFees;
                    }
                }

                decimal unityInstallFeeCosts = CalculateMonthlyUnityAdminFeesInUSD(license, totalNumberOfInstalls, installsWithFeePaid, installsWithoutFeePaid, totalGrossRevenueUSD);

                decimal unityLicenseFeeCosts = MonthlyData[i].NumberOfEmployees * license.GetMonthlyLicenseCosts(false);
                var monthlyEmployeeCostsUSD = (MonthlyData[i].NumberOfEmployees * MonthlyData[i].TotalEmployerSalaries) / 12M;

                var totalMonthlyCostsUSD = unityInstallFeeCosts + unityLicenseFeeCosts + monthlyEmployeeCostsUSD;

                var monthlyNettRevenueUSD = monthlyGrossRevenue - totalMonthlyCostsUSD;
                var totalNettRevenueUSD = i > 0 ? costResults[i - 1].TotalNettRevenueUSD + monthlyNettRevenueUSD : monthlyNettRevenueUSD;

                var result = new FreeToPlayEstimatesResult(totalNumberOfInstalls, averageRevenuePerInstallUSD, monthlyGrossRevenue, totalGrossRevenueUSD,
                                                                        unityInstallFeeCosts, unityLicenseFeeCosts, monthlyEmployeeCostsUSD, monthlyNettRevenueUSD, totalNettRevenueUSD);

                costResults.Add(result);
            }

            return costResults;
        }

        public static bool HasFeeThresholdReachedBeenReached(UnityLicenseType licenseType, uint totalNumberOfInstalls, decimal totalGrossRevenueUSD)
        {
            var licenseData = UnityLicenses.LicensesByType[licenseType];

            return HasFeeThresholdReachedBeenReached(licenseData, totalNumberOfInstalls, totalGrossRevenueUSD);
        }

        public static bool HasFeeThresholdReachedBeenReached(UnityLicense licenseData, uint totalNumberOfInstalls, decimal totalGrossRevenueUSD)
        {
            if (totalGrossRevenueUSD < licenseData.RevenueUSDThresholdForFees)
            {
                return false;
            }

            if (totalNumberOfInstalls < licenseData.InstallThresholdForFees)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        public static decimal CalculateMonthlyUnityAdminFeesInUSD(UnityLicenseType licenseType, uint totalNumberOfInstalls, uint installsWithFeePaid, uint installsWithoutFeePaid, decimal totalGrossRevenueUSD)
        {
            var licenseData = UnityLicenses.LicensesByType[licenseType];

            return CalculateMonthlyUnityAdminFeesInUSD(licenseData, totalNumberOfInstalls, installsWithFeePaid, installsWithoutFeePaid, totalGrossRevenueUSD);
        }

        public static decimal CalculateMonthlyUnityAdminFeesInUSD(UnityLicense licenseData, uint totalNumberOfInstalls, uint installsWithFeePaid, uint installsWithoutFeePaid, decimal totalGrossRevenueUSD)
        {
            if (HasFeeThresholdReachedBeenReached(licenseData, totalNumberOfInstalls, totalGrossRevenueUSD))
            {
                decimal totalFeeCostsUSD = 0;
                for (uint installNumber = installsWithFeePaid; installNumber < totalNumberOfInstalls; ++installNumber)
                {
                    decimal feeCostUSD = 0;
                    foreach (var feeData in licenseData.InstallFeeThresholds.Fees)
                    {
                        if (installNumber >= feeData.MinimumInstallsOverThreshold && installNumber <= feeData.MaximumInstallsOverThreshold)
                        {
                            feeCostUSD = feeData.InstallFeeUSD;
                            break;
                        }
                    }

                    totalFeeCostsUSD += feeCostUSD;
                }

                return totalFeeCostsUSD;
            }
            else
            {
                return 0;
            }
        }
    }
}