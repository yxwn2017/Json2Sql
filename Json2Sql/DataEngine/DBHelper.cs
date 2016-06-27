using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Json2Sql.DataEngine
{
    public class DBHelper
    {
        public DBHelper() { }

        public DBHelper(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public string ConnectionString { get; set; }

        public int ExcuteSql(string sql) => ExcuteSql(sql, ConnectionString);


        public T QuerySql<T>(string sql) where T : class => QuerySql<T>(sql, ConnectionString);

        public List<Dictionary<string, object>> Query(string sql) => QuerySql<List<Dictionary<string, object>>>(sql);

        public int ExcuteSql(string sql, string connectionString)
        {
            var dbconnection = new MySqlConnection(connectionString);
            var dbcommand = new MySqlCommand();

            dbcommand.Connection = dbconnection;
            if (dbconnection.State == ConnectionState.Closed) dbconnection.Open();

            dbcommand.Transaction = dbconnection.BeginTransaction();
            dbcommand.CommandText = sql;

            try
            {
                var i = dbcommand.ExecuteNonQuery();
                dbcommand.Transaction.Commit();
                dbconnection.Close();
                return i;
            }
            catch (Exception ex)
            {
                dbcommand.Transaction.Rollback();
                throw ex;
            }
            finally
            {
                dbconnection.Close();
            }
        }

        public T QuerySql<T>(string sql, string connectionString) where T : class
        {
            var dbconnection = new MySqlConnection(connectionString);
            try
            {
                var dbcommand = new MySqlCommand();
                dbcommand.Connection = dbconnection;
                var dba = new MySqlDataAdapter(dbcommand);
                if (dbconnection.State == ConnectionState.Closed) dbconnection.Open();
                dbcommand.CommandText = $"{sql};";
                var dt = new DataTable();
                dba.Fill(dt);
                var json = Json2KeyValue.JsonConvert.SerializeObject(dt.ToDictionary("yyyy/MM/dd HH:mm:ss"));
                dbconnection.Close();
                return Json2KeyValue.JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbconnection.Close();
            }
        }

        public List<Dictionary<string, object>> Query(string sql, string connectionString) => QuerySql<List<Dictionary<string, object>>>(sql, connectionString);

    }
}
