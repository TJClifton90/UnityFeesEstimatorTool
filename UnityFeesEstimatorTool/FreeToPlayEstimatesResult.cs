namespace UnityFeesEstimatorTool
{
    class FreeToPlayEstimatesResult
    {
        public readonly uint TotalNumberOfInstalls;
        public readonly decimal AverageRevenuePerInstallUSD;
        public readonly decimal MonthlyGrossRevenueUSD;
        public readonly decimal TotalGrossRevenueUSD;
        public readonly decimal UnityInstallFeeCostsUSD;
        public readonly decimal UnityLicenseFeeCostsUSD;
        public readonly decimal EmployeeCostsUSD;
        public readonly decimal MonthlyNettRevenueUSD;
        public readonly decimal TotalNettRevenueUSD;

        public FreeToPlayEstimatesResult(uint totalNumberOfInstalls, decimal averageRevenuePerInstallUSD, decimal monthlyGrossRevenueUSD,
                                         decimal totalGrossRevenueUSD, decimal unityInstallFeeCostsUSD, decimal unityLicenseFeeCostsUSD,
                                         decimal employeeCostsUSD, decimal monthlyNettRevenueUSD, decimal totalNettRevenueUSD)
        {
            TotalNumberOfInstalls = totalNumberOfInstalls;
            AverageRevenuePerInstallUSD = averageRevenuePerInstallUSD;
            MonthlyGrossRevenueUSD = monthlyGrossRevenueUSD;
            TotalGrossRevenueUSD = totalGrossRevenueUSD;
            UnityInstallFeeCostsUSD = unityInstallFeeCostsUSD;
            UnityLicenseFeeCostsUSD = unityLicenseFeeCostsUSD;
            EmployeeCostsUSD = employeeCostsUSD;
            MonthlyNettRevenueUSD = monthlyNettRevenueUSD;
            TotalNettRevenueUSD = totalNettRevenueUSD;
        }
    }
}