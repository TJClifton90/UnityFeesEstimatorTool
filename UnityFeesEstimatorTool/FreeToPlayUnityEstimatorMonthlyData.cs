namespace TJClifton.UnityFeesEstimatorTool
{
    public class FreeToPlayUnityEstimatorMonthlyData
    {
        public uint NumberOfInstalls = 100;
        public decimal LTV = 0.01M;
        public decimal CPI = 0;
        public uint NumberOfEmployees = 1;
        public uint TotalEmployerSalaries = 0;

        public FreeToPlayUnityEstimatorMonthlyData() { }
        public FreeToPlayUnityEstimatorMonthlyData(FreeToPlayUnityEstimatorMonthlyData other)
        {
            NumberOfInstalls = other.NumberOfInstalls;
            LTV = other.LTV;
            CPI = other.CPI;
            NumberOfEmployees = other.NumberOfEmployees;
            TotalEmployerSalaries = other.TotalEmployerSalaries;
        }
    }
}