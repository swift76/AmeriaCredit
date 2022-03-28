using System;
using System.Data;
using Dapper;
using IntelART.Ameria.Entities;

namespace IntelART.Ameria.Repositories
{
    public class CardValidationRepository : BaseRepository
    {
        public CardValidationRepository(string connectionString) : base(connectionString)
        {
        }

        public ClientCard ValidateCardData(string clientCode, string cardNumber, DateTime expiryDate)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("CLICODE", clientCode);
            parameters.Add("CARDNUM", cardNumber);
            parameters.Add("EXPIRY", expiryDate);
            return GetSingle<ClientCard>(parameters, string.Format("{0}dbo.am0sp_GetClientCardData", GetSetting("BANK_SERVER_DATABASE")));
        }

        public bool IsCardActive(string cardNumber, DateTime expiryDate)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("cardNumber", cardNumber);
            parameters.Add("expiryDate", expiryDate);
            int status = GetSingle<int>(parameters, "select Common.f_GetCardStatus(@cardNumber,@expiryDate)", cmdType: CommandType.Text);
            return (status != 1 && status != 3 && status != 6 && status != 7 && status != 8 && status != 9 && status != 10 && status != 12 && status != 19 && status != 20 && status != 21);
        }
    }
}
