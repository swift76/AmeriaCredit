using System;
using System.Collections.Generic;

namespace IntelART.Ameria.CLRServices
{
    public class ACRAData
    {
        public ACRAQueryResult Main { get; set; }
        public ACRAQueryResult Coborrower { get; set; }
    }
    
    public class ACRAQueryResult
    {
        public string FicoScore { get; set; }
        public List<ACRAQueryResultDetails> Details { get; set; }
        public List<ACRAQueryResultQueries> Queries { get; set; }
        public List<ACRAQueryResultInterrelated> Interrelated { get; set; }
        public string XML { get; set; }
        public bool IsBlocked { get; set; }

        public ACRAQueryResult()
        {
            Details = new List<ACRAQueryResultDetails>();
            Queries = new List<ACRAQueryResultQueries>();
            Interrelated = new List<ACRAQueryResultInterrelated>();
        }
    }
}
