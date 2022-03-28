using Dapper;
using System.Collections.Generic;
using IntelART.Ameria.Entities;

namespace IntelART.Ameria.Repositories
{
    public class ShopRepository : BaseRepository
    {
        public ShopRepository(string connectionString) : base(connectionString)
        {
        }

        public IEnumerable<Shop> GetShops()
        {
            return GetList<Shop>(new DynamicParameters(), "IL.sp_GetShops");
        }
    }
}
