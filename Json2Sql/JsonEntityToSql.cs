using Json2Sql.ConditionModel;
using Json2Sql.ModelEx;
using MySql.Data.MySqlClient;

namespace Json2Sql
{
    public class JsonEntityToSql : IJsonSql
    {
        private string ConnectionString { get; set; }
        private string DbName { get; set; }


        /// <summary>
        /// 初始化一个Json实体转为MySql的实例。
        /// </summary>
        /// <param name="dbServerIP">数据库IP地址</param>
        /// <param name="dbUserId">数据库用户名</param>
        /// <param name="dbPassword">数据库密码</param>
        /// <param name="dbName">数据库名</param>
        public JsonEntityToSql(string dbServerIP, string dbUserId, string dbPassword, string dbName)
        {
            DbName = dbName;
            ConnectionString = $"server={dbServerIP}; port=3306;user id={dbUserId};password={dbPassword};persistsecurityinfo=True;database={dbName}";
        }

        /// <summary>
        /// 初始化一个Json实体转为MySql的实例。
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        public JsonEntityToSql(string connectionString)
        {
            ConnectionString = connectionString;

        }

        /// <summary>
        /// 将json转为delete 语句的sql
        /// </summary>
        /// <param name="json">json字符串</param>
        /// <returns>返回json语句</returns>
        public string Json2DeleteSql(string json)
        {
            var dic = json.ToJsonDictionary();
            var sqlString = new SqlString(dic)
            {
                MySqlConnection = new MySqlConnection(ConnectionString),
                //DataBaseName = DbName
            };
            return sqlString.GetDeleteSqls();
        }

        /// <summary>
        /// 将json转为insert into 语句的sql
        /// </summary>
        /// <param name="json">json字符串</param>
        /// <returns>返回json语句</returns>
        public string Json2InsertSql(string json)
        {
            var dic = json.ToJsonDictionary();
            var sqlString = new SqlString(dic)
            {
                MySqlConnection = new MySqlConnection(ConnectionString),
                //DataBaseName = DbName
            };
            return sqlString.GetInsertSqls();
        }

        /// <summary>
        /// 将json转为slect语句的sql
        /// </summary>
        /// <param name="json"></param>
        /// <returns>返回json语句</returns>
        public string Json2SelectSql(string json)
        {
            var dic = json.ToJsonDictionary();
            var sqlString = new SqlString(dic)
            {
                MySqlConnection = new MySqlConnection(ConnectionString),
                //DataBaseName = DbName
            };
            return sqlString.GetQuerySql();
        }

        /// <summary>
        /// 将json转为update语句的sql
        /// </summary>
        /// <param name="json">json字符串</param>
        /// <returns>返回json语句</returns>
        public string Json2UpdateSql(string json)
        {
            var dic = json.ToJsonDictionary();
            var sqlString = new SqlString(dic)
            {
                MySqlConnection = new MySqlConnection(ConnectionString),
                //DataBaseName = DbName
            };
            return sqlString.GetUpdateSqls();
        }
    }
}
