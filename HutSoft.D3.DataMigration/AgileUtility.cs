using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;

namespace HutSoft.D3.DataMigration
{
    internal enum DatabaseType
    {
        SQLite,
        Oracle
    }

    internal class AgileUtility
    {
        private Settings _settings;
        private DatabaseType _databaseType;

        internal AgileUtility(Settings settings)
        {
            _settings = settings;
        }

        internal Settings Settings
        {
            get { return _settings; }
            set { _settings = value; }
        }

        internal DatabaseType DatabaseType
        {
            get { return _databaseType; }
            set { _databaseType = value; }
        }

        internal bool TestSQLiteConnection()
        {
            return TestSQLiteConnection(_settings.AgileSQLiteConnectionString);
        }

        internal bool TestSQLiteConnection(string connectionString)
        {
            bool result = false;
            try
            {
                DataTable dt = GetDataTableFromSQLite("SELECT 1", connectionString);
                if (dt.Rows.Count > 0)
                    result = true;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return result;
        }

        internal bool TestOracleConnection()
        {
            return TestOracleConnection(_settings.AgileOracleConnectionString);
        }

        internal bool TestOracleConnection(string connectionString)
        {
            bool result = false;
            try
            {
                DataTable dt = GetDataTableFromOracle("SELECT 1", connectionString);
                if (dt.Rows.Count > 0)
                    result = true;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return result;
        }

        internal DataTable GetStatuses()
        {
            string sql =
               "SELECT " +
               "   CASE " +
               "      WHEN STATUS IS NULL OR STATUS = \"\" THEN \"NOT MIGRATED\" " +
               "      ELSE STATUS " +
               "   END AS STATUS, " +
               "   STATUS_COUNT " +
               "FROM ( " +
               "   SELECT VAULT_STATUS AS STATUS, COUNT(*) AS STATUS_COUNT " +
               "   FROM AED_ITEM_FILES " +
               "   WHERE FILE_LOCATIONS = \"iFS\" " +
               "   GROUP BY VAULT_STATUS " +
               "   UNION " +
               "   SELECT \"TOTAL\" AS STATUS, COUNT(*) AS STATUS_COUNT " +
               "   FROM AED_ITEM_FILES " +
               "   WHERE FILE_LOCATIONS = \"iFS\" " +
               ") TBL";

            return GetDataTable(sql);
        }

        internal DataTable GetTop100Sample()
        {
            string sql =
                "SELECT * " +
                "FROM AED_ITEM_FILES " +
                "WHERE FILE_LOCATIONS = \"iFS\" " +
                "LIMIT 100";
            return GetDataTable(sql);
        }

        internal DataTable GetDistinctFileIds()
        {
            string sql = string.Format(
                "SELECT DISTINCT FILE_ID " +
                "FROM AED_ITEM_FILES " +
                "WHERE FILE_LOCATIONS = \"iFS\" " +
                "AND COALESCE(VAULT_STATUS, \"\") <> \"{0}\" ",
                _settings.ReleasedStateName);
            return GetDataTable(sql);
        }

        internal DataTable GetFileByFileId(string fileID)
        {
            string sql = string.Format(
                "SELECT * " +
                "FROM AED_ITEM_FILES " +
                "WHERE FILE_LOCATIONS = \"iFS\" " + "" +
                "AND FILE_ID = {0} " +
                "AND COALESCE(VAULT_STATUS, \"\") <>  \"{1}\" " +
                "ORDER BY \"?REV_ID\"",
                fileID,
                _settings.ReleasedStateName);
            return GetDataTable(sql);
        }

        internal void WriteBackFusionStatus(string fileID, string revID, string fusionStatus)
        {
            string sql = string.Format(
                "UPDATE AED_ITEM_FILES " +
                "SET FUSION_STATUS = {0} " +
                "WHERE FILE_LOCATIONS = \"iFS\" " + "" +
                "AND FILE_ID = {1} " +
                "AND \"?REV_ID\" = {2}",
                fusionStatus,
                fileID,
                revID);
             ExecuteNonQuery(sql);
        }

        internal void WriteBackVaultFileInfo(string fileID, string revID, long revMasterId, long revFileId)
        {
            //TODO: Missing VAULT_VERSION_FILE_ID in current agile database schema
            string sql = string.Format(
                "UPDATE AED_ITEM_FILES " +
                "SET VAULT_FILE_ID = {0}, " +
                "VAULT_VERSION_FILE_ID = {1}, " +
                "FUSION_STATUS = {2} " +
                "WHERE FILE_LOCATIONS = \"iFS\" " + "" +
                "AND FILE_ID = {3} " +
                "AND \"?REV_ID\" = {4}",
                revMasterId,
                revFileId,
                _settings.ReleasedStateName,
                fileID,
                revID);
            ExecuteNonQuery(sql);
        }

        private DataTable GetDataTable(string sql)
        {
            DataTable dt = null;
            switch (_databaseType)
            {
                case DatabaseType.SQLite:
                    dt = GetDataTableFromSQLite(sql, _settings.AgileSQLiteConnectionString);
                    break;
                case DatabaseType.Oracle:
                    dt = GetDataTableFromOracle(sql, _settings.AgileOracleConnectionString);
                    break;
            }
            return dt;
        }

        private DataTable GetDataTableFromSQLite(string sql, string connectionString)
        {
            DataTable dt = null;
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    using (SQLiteDataAdapter da = new SQLiteDataAdapter(sql, conn))
                    {
                        dt = new DataTable();
                        da.Fill(dt);
                    }
                    conn.Close();
                }
            }
            catch (SQLiteException ex)
            {
                throw (ex);
            }
            return dt;
        }

        private DataTable GetDataTableFromOracle(string sql, string connectionString)
        {
            DataTable dt = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlDataAdapter da = new SqlDataAdapter(sql, conn))
                    {
                        dt = new DataTable();
                        da.Fill(dt);
                    }
                    conn.Close();
                }
            }
            catch (SqlException ex)
            {
                throw (ex);
            }
            return dt;
        }

        private void ExecuteNonQuery(string sql)
        {
            switch (_databaseType)
            {
                case DatabaseType.SQLite:
                    ExecuteNonQueryFromSQLite(sql, _settings.AgileSQLiteConnectionString);
                    break;
                case DatabaseType.Oracle:
                    ExecuteNonQueryFromOracle(sql, _settings.AgileOracleConnectionString);
                    break;
            }
        }

        private void ExecuteNonQueryFromSQLite(string sql, string connectionString)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    SQLiteCommand cmd = conn.CreateCommand();
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (SQLiteException ex)
            {
                throw (ex);
            }
        }

        private void ExecuteNonQueryFromOracle(string sql, string connectionString)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (SqlException ex)
            {
                throw (ex);
            }
        }
    }
}
