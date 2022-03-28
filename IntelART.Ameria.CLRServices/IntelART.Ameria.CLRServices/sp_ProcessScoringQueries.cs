using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using IntelART.Ameria.CLRServices;

public partial class StoredProcedures
{
    [SqlProcedure]
    public static void sp_ProcessScoringQueries(SqlInt32 queryTimeout)
    {
        DataHelper.CommandTimeoutInterval = queryTimeout.Value;
        ServiceHelper.QueryTimeout = queryTimeout.Value;
        using (DataHelper helper = new DataHelper())
        {
            try
            {
                DoNORQQueries(helper, helper.GetApplicationsForNORQRequest(), queryTimeout.Value);
            }
            catch (Exception ex)
            {
                helper.LogError("NORQ Query", ex.ToString());
            }

            try
            {
                DoACRAQueries(helper, helper.GetApplicationsForACRARequest(), queryTimeout.Value);
            }
            catch (Exception ex)
            {
                helper.LogError("ACRA Query", ex.ToString());
            }
        }
    }

    [SqlProcedure]
    public static void sp_ProcessScoringQueriesByID(SqlInt32 queryTimeout, SqlGuid id)
    {
        DataHelper.CommandTimeoutInterval = queryTimeout.Value;
        ServiceHelper.QueryTimeout = queryTimeout.Value;
        using (DataHelper helper = new DataHelper())
        {
            try
            {
                DoNORQQueries(helper, helper.GetApplicationForNORQRequestByID(id.Value), queryTimeout.Value);
            }
            catch (Exception ex)
            {
                helper.LogError("NORQ Query", ex.ToString());
            }

            try
            {
                DoACRAQueries(helper, helper.GetApplicationForACRARequestByID(id.Value), queryTimeout.Value);
            }
            catch (Exception ex)
            {
                helper.LogError("ACRA Query", ex.ToString());
            }
        }
    }

    [SqlProcedure]
    public static void sp_ProcessScoringQueriesByISN(SqlInt32 queryTimeout, SqlInt32 isn)
    {
        DataHelper.CommandTimeoutInterval = queryTimeout.Value;
        ServiceHelper.QueryTimeout = queryTimeout.Value;
        using (DataHelper helper = new DataHelper())
        {
            try
            {
                DoNORQQueries(helper, helper.GetApplicationForNORQRequestByISN(isn.Value), queryTimeout.Value);
            }
            catch (Exception ex)
            {
                helper.LogError("NORQ Query", ex.ToString());
            }

            try
            {
                DoACRAQueries(helper, helper.GetApplicationForACRARequestByISN(isn.Value), queryTimeout.Value);
            }
            catch (Exception ex)
            {
                helper.LogError("ACRA Query", ex.ToString());
            }
        }
    }

    [SqlProcedure]
    public static void sp_ProcessNORQQueryByID(SqlInt32 queryTimeout, SqlGuid id)
    {
        DataHelper.CommandTimeoutInterval = queryTimeout.Value;
        ServiceHelper.QueryTimeout = queryTimeout.Value;
        using (DataHelper helper = new DataHelper())
        {
            try
            {
                DoNORQQueries(helper, helper.GetApplicationForNORQRequestByID(id.Value), queryTimeout.Value);
            }
            catch (Exception ex)
            {
                helper.LogError("NORQ Query", ex.ToString());
            }
        }
    }

    [SqlProcedure]
    public static void sp_ProcessACRAQueryByID(SqlInt32 queryTimeout, SqlGuid id)
    {
        DataHelper.CommandTimeoutInterval = queryTimeout.Value;
        ServiceHelper.QueryTimeout = queryTimeout.Value;
        using (DataHelper helper = new DataHelper())
        {
            try
            {
                DoACRAQueries(helper, helper.GetApplicationsForACRARequest(), queryTimeout.Value);
            }
            catch (Exception ex)
            {
                helper.LogError("ACRA Query", ex.ToString());
            }
        }
    }

    [SqlProcedure]
    public static void sp_SaveNORQResult(SqlInt32 queryTimeout, SqlString socialCard, SqlString userID)
    {
        DataHelper.CommandTimeoutInterval = queryTimeout.Value;
        ServiceHelper.QueryTimeout = queryTimeout.Value;
        using (DataHelper helper = new DataHelper())
        {
            ServiceConfig config = helper.GetServiceConfig("NORQ");
            try
            {
                NORQQueryResult result = (new NORQQuery()).GetNORQResult(helper, config, socialCard.Value, queryTimeout.Value);
                result.FirstName = GetNameString(result.FirstName);
                result.LastName = GetNameString(result.LastName);
                result.MiddleName = GetNameString(result.MiddleName);
                helper.SaveNORQClientQueryResult(result, userID.Value);
            }
            catch (Exception ex)
            {
                helper.LogError("NORQ Query", ex.ToString());
            }
        }
    }

    [SqlProcedure]
    public static void sp_SaveACRAResult(SqlInt32 queryTimeout, SqlString socialCard, SqlString userID, SqlString firstName, SqlString lastName, SqlDateTime birthDate, SqlString passport, SqlString idCard)
    {
        DataHelper.CommandTimeoutInterval = queryTimeout.Value;
        ServiceHelper.QueryTimeout = queryTimeout.Value;
        using (DataHelper helper = new DataHelper())
        {
            ServiceConfig config = helper.GetServiceConfig("ACRA");
            try
            {
                ACRALoginResult loginResult = ServiceHelper.ACRALogin(config, queryTimeout.Value, "acra_monitoring");
                if (loginResult.Response != "OK")
                    throw new ApplicationException(loginResult.Error);

                string firstNameUTF = StringTranlator.ToUnicode(firstName.Value);
                string lastNameUTF = StringTranlator.ToUnicode(lastName.Value);

                ACRAQueryResult result = (new ACRAQuery()).GetACRAResult(helper, config, loginResult.SID, "acra_monitoring", socialCard.Value, firstNameUTF, lastNameUTF, birthDate.Value, passport.Value, idCard.Value, 0, queryTimeout.Value);
                if (result != null)
                    helper.SaveACRAClientQueryResult(result, socialCard.Value, firstNameUTF, lastNameUTF, birthDate.Value, passport.Value, idCard.Value, userID.Value);
            }
            catch (Exception ex)
            {
                helper.LogError("ACRA Query", ex.ToString());
            }
        }
    }

    private static string GetNameString(string source)
    {
        string result = string.Format("{0}{1}", source.Substring(0,1), source.Substring(1).ToLower());
        result = result.Replace("եվ", "և");
        return result;
    }

    private static void DoNORQQueries(DataHelper helper, List<NORQEntity> entities_NORQ, int queryTimeout)
    {
        ServiceConfig config = helper.GetServiceConfig("NORQ");
        if (entities_NORQ.Count > 0)
        {
            NORQQuery norqQuery = new NORQQuery();
            foreach (NORQEntity entity in entities_NORQ)
            {
                try
                {
                    norqQuery.GetResponse(helper, config, entity, queryTimeout);
                }
                catch (Exception ex)
                {
                    helper.LogError("NORQ Query", ex.ToString(), entity.ID);
                }
            }
        }
    }

    private static void DoACRAQueries(DataHelper helper, List<ACRAEntity> entities_ACRA, int queryTimeout)
    {
        ServiceConfig config = helper.GetServiceConfig("ACRA");
        if (entities_ACRA.Count > 0)
        {
            ACRALoginResult loginResult = ServiceHelper.ACRALogin(config, queryTimeout, "acra_service");
            if (loginResult.Response != "OK")
                throw new ApplicationException(loginResult.Error);

            ACRAQuery acraQuery = new ACRAQuery();
            foreach (ACRAEntity entity in entities_ACRA)
                try
                {
                    acraQuery.GetResponse(helper, config, loginResult.SID, entity, queryTimeout);
                }
                catch (Exception ex)
                {
                    helper.LogError("ACRA Query", ex.ToString(), entity.ID);
                }
        }
    }
};
