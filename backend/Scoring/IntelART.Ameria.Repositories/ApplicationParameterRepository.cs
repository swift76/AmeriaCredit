using System.Threading.Tasks;
using Dapper;
using IntelART.Ameria.Entities;
using System.Collections.Generic;

namespace IntelART.Ameria.Repositories
{
    public class ApplicationParameterRepository : BaseRepository
    {
        public ApplicationParameterRepository(string connectionString) : base(connectionString)
        {
        }

        public async Task<LoanLimits> GetLoanLimits(string loanTypeCode, string currency)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("LOAN_TYPE_CODE", loanTypeCode);
            parameters.Add("CURRENCY", currency);
            return await GetSingleAsync<LoanLimits>(parameters, "Common.sp_GetLoanLimits");
        }

        public async Task<LoanParameters> GetLoanParameters(string loanTypeCode)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("LOAN_TYPE_CODE", loanTypeCode);
            return await GetSingleAsync<LoanParameters>(parameters, "Common.sp_GetLoanParameters");
        }

        public async Task<IEnumerable<Shop>> GetParentShops()
        {
            return await GetListAsync<Shop>(new DynamicParameters(), "IL.sp_GetParentShops");
        }

        public async Task<RepaymentSchedule> GetLoanMonthlyPaymentAmount(decimal interest, decimal amount, byte term, decimal serviceInterest, decimal serviceAmount)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("AMOUNT", amount);
            parameters.Add("TERM", term);
            parameters.Add("INTEREST", interest);
            parameters.Add("SERVICE_AMOUNT", serviceAmount);
            parameters.Add("SERVICE_INTEREST", serviceInterest);
            return await GetSingleAsync<RepaymentSchedule>(parameters, "Common.sp_GetLoanMonthlyPaymentAmount");
        }

        public int GetFileMaxSize()
        {
            int fileMaxSize = int.Parse(GetSetting("FILE_MAX_SIZE"));
            return fileMaxSize;
        }
    }
}
