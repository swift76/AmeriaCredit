using System;
using System.Collections.Generic;
using Dapper;

namespace IntelART.Ameria.Repositories
{
    public class BankDataRepository : BaseRepository
    {
        private string BankDB;

        public BankDataRepository(string connectionString) : base(connectionString)
        {
            BankDB = ;
        }

    }
}
