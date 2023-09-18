namespace TJClifton.UnityFeesEstimatorTool
{
    public class FreeToPlayCostEstimator
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

                uint numberOfInstallsOverThreshold = totalNumberOfInstalls - installsWithFeePaid;

                var feesData = CalculateMonthlyUnityAdminFeesInUSD(license, totalNumberOfInstalls, numberOfInstallsOverThreshold, totalGrossRevenueUSD);
                installsWithFeePaid += feesData.numberOfFeesPaid;

                decimal unityLicenseFeeCosts = MonthlyData[i].NumberOfEmployees * license.GetMonthlyLicenseCosts(false);
                var monthlyEmployeeCostsUSD = (MonthlyData[i].NumberOfEmployees * MonthlyData[i].TotalEmployerSalaries) / 12M;

                var totalMonthlyCostsUSD = feesData.totalFeeCostsUSD + unityLicenseFeeCosts + monthlyEmployeeCostsUSD;

                var monthlyNettRevenueUSD = monthlyGrossRevenue - totalMonthlyCostsUSD;
                var totalNettRevenueUSD = i > 0 ? costResults[i - 1].TotalNettRevenueUSD + monthlyNettRevenueUSD : monthlyNettRevenueUSD;

                var result = new FreeToPlayEstimatesResult(totalNumberOfInstalls, averageRevenuePerInstallUSD, monthlyGrossRevenue, totalGrossRevenueUSD,
                                                           feesData.totalFeeCostsUSD, unityLicenseFeeCosts, monthlyEmployeeCostsUSD, monthlyNettRevenueUSD, totalNettRevenueUSD);

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


        public static (decimal totalFeeCostsUSD, uint numberOfFeesPaid) CalculateMonthlyUnityAdminFeesInUSD(UnityLicenseType licenseType, uint totalNumberOfInstalls, uint numberOfInstallsOverThreshold, decimal totalGrossRevenueUSD)
        {
            var licenseData = UnityLicenses.LicensesByType[licenseType];

            return CalculateMonthlyUnityAdminFeesInUSD(licenseData, totalNumberOfInstalls, numberOfInstallsOverThreshold, totalGrossRevenueUSD);
        }

        public static (decimal totalFeeCostsUSD, uint numberOfFeesPaid) CalculateMonthlyUnityAdminFeesInUSD(UnityLicense licenseData, uint totalNumberOfInstalls, uint numberOfInstallsOverThreshold, decimal totalGrossRevenueUSD)
        {
            if (HasFeeThresholdReachedBeenReached(licenseData, totalNumberOfInstalls, totalGrossRevenueUSD))
            {
                //decimal totalFeeCostsUSD = 0;
                //uint installNumber = installsWithFeePaid;

                //foreach (var feeData in licenseData.InstallFeeThresholds.Fees)
                //{
                //    if (installNumber >= feeData.MinimumInstallsOverThreshold && installNumber <= feeData.MaximumInstallsOverThreshold)
                //    {
                //        uint numberOfFeesToPay = (uint)MathF.Min((feeData.MaximumInstallsOverThreshold - installNumber), installsWithoutFeePaid);

                //        totalFeeCostsUSD += numberOfFeesToPay * feeData.InstallFeeUSD;
                //    }
                //}

                //iterate over each fee data
                //is current install amount in that threshold
                //add relevant number
                //move to next fee info

                decimal totalFeeCostsUSD = 0;
                uint totalFeesPaid = 0;

                foreach (var feeData in licenseData.InstallFeeThresholds.Fees)
                {
                    uint numberOfInstallsInTHresholdRange = (feeData.MaximumInstallsOverThreshold + 1) - feeData.MinimumInstallsOverThreshold;

                    var numberOfInstallsToChargeAtCurrentFee = (uint)MathF.Min(numberOfInstallsOverThreshold, numberOfInstallsInTHresholdRange);

                    totalFeeCostsUSD += numberOfInstallsToChargeAtCurrentFee * feeData.InstallFeeUSD;
                    totalFeesPaid += numberOfInstallsToChargeAtCurrentFee;

                    numberOfInstallsOverThreshold -= numberOfInstallsToChargeAtCurrentFee;
                }

                return (totalFeeCostsUSD, totalFeesPaid);


                //decimal totalFeeCostsUSD = 0;
                //for (uint installNumber = installsWithFeePaid; installNumber < totalNumberOfInstalls; ++installNumber)
                //{
                //    decimal feeCostUSD = 0;
                //    foreach (var feeData in licenseData.InstallFeeThresholds.Fees)
                //    {
                //        if (installNumber >= feeData.MinimumInstallsOverThreshold && installNumber <= feeData.MaximumInstallsOverThreshold)
                //        {
                //            feeCostUSD = feeData.InstallFeeUSD;
                //            break;
                //        }
                //    }

                //    //if (feeCostUSD == 0)
                //    //{
                //    //    return totalFeeCostsUSD;
                //    //}

                //    totalFeeCostsUSD += feeCostUSD;
                //}

                //return totalFeeCostsUSD;
            }
            else
            {
                return (0, 0);
            }
        }
    }
}