using System;
using System.Data.SqlTypes;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Microsoft.SqlServer.Server;
using IntelART.Ameria.CLRServices;

public partial class StoredProcedures
{
    [SqlProcedure]
    public static void sp_GetILScoringResult(SqlInt32 queryTimeout, SqlString id)
    {
        using (DataHelper helper = new DataHelper())
        {
            try
            {
                ScoringResult result = new ScoringResult();
                result.ID = new Guid(id.ToString());
                ServiceConfig config = helper.GetServiceConfig("ISCR");
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}?id={1}", config.URL, id));
                request.ContentType = "application/x-www-form-urlencoded";
                request.Timeout = 1000 * queryTimeout.Value;
                request.ReadWriteTimeout = request.Timeout;
                request.Method = "GET";
                WebResponse response = request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    string responseText = reader.ReadToEnd();
                    responseText = responseText.Replace("{", string.Empty).Replace("}", string.Empty);
                    string[] resultArray = responseText.Split(',');
                    for (int i = 0; i < resultArray.Length; i++)
                    {
                        string[] valueArray = resultArray[i].Split(':');
                        switch (valueArray[0].ToLower())
                        {
                            case "\"amount\"":
                                result.Amount = decimal.Parse(valueArray[1]);
                                break;
                            case "\"pd\"":
                                result.Coefficient = decimal.Parse(valueArray[1]);
                                break;
                        }
                    }
                }
                helper.SaveScoringResult(result);
            }
            catch (Exception ex)
            {
                helper.LogError("Scoring Query", ex.ToString());
                throw new ApplicationException(ex.Message);
            }
        }
    }

    [SqlProcedure]
    public static void sp_GetGLScoringResult(SqlInt32 queryTimeout, SqlString id
        , SqlDecimal amountWithCoefficient1, SqlDecimal amountWithCoefficient2, SqlDecimal amountForOTI, SqlDecimal limitAmount
        , SqlDecimal amountForAverageRemainder, SqlDecimal averageRemainder, SqlDecimal bankSalary
        , SqlDecimal maxInterestRate, SqlDecimal minInterestRate, SqlDecimal limitAmount3)
    {
        using (DataHelper helper = new DataHelper())
        {
            try
            {
                List<ScoringResult> results = new List<ScoringResult>();

                ServiceConfig config = helper.GetServiceConfig("GSCR");
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}?id={1}&patik1={2}&patik2={3}&a_oti={4}&azat={5}&patik_avrem={6}&avrem={7}&ameria_income={8}&maxir={9}&minir={10}&azat3={11}", config.URL, id
                   , amountWithCoefficient1.Value, amountWithCoefficient2.Value, amountForOTI.Value, limitAmount.Value, amountForAverageRemainder.Value, averageRemainder.Value, bankSalary.Value, maxInterestRate.Value / 100, minInterestRate.Value / 100, limitAmount3.Value));
                request.ContentType = "application/x-www-form-urlencoded";
                request.Timeout = 1000 * queryTimeout.Value;
                request.ReadWriteTimeout = request.Timeout;
                request.Method = "GET";
                WebResponse response = request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    string responseText = reader.ReadToEnd();
                    responseText = Regex.Replace(responseText.Replace("[", string.Empty).Replace("]", string.Empty), @"\s+", string.Empty);
                    string[] optionArray = responseText.Split(new[] { "},{" }, StringSplitOptions.RemoveEmptyEntries);
                    for (int j = 0; j < optionArray.Length; j++)
                    {
                        ScoringResult result = new ScoringResult();
                        responseText = optionArray[j].Replace("{", string.Empty).Replace("}", string.Empty);
                        string[] resultArray = responseText.Split(',');
                        for (int i = 0; i < resultArray.Length; i++)
                        {
                            string[] valueArray = resultArray[i].Split(':');
                            switch (valueArray[0].ToLower())
                            {
                                case "\"scoretype\"":
                                    result.Option = byte.Parse(valueArray[1].Substring(1, 1));
                                    break;
                                case "\"amount\"":
                                    result.Amount = decimal.Parse(valueArray[1]);
                                    break;
                                case "\"pd\"":
                                    result.Coefficient = decimal.Parse(valueArray[1]);
                                    break;
                                case "\"interestrate\"":
                                    result.Interest = 100 * decimal.Parse(valueArray[1]);
                                    break;
                            }
                        }
                        results.Add(result);
                    }
                }
                helper.SaveScoringResults(new Guid(id.ToString()), results);
            }
            catch (Exception ex)
            {
                helper.LogError("Scoring Query", ex.ToString());
                throw new ApplicationException(ex.Message);
            }
        }
    }
};
