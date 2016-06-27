using Json2Sql.ConditionModel;
using Json2Sql.ModelEx;
using MySql.Data.MySqlClient;

namespace Json2Sql
{
    public class JsonEntityToSql : IJsonSql
    {
        private string Information_schemaConnectionString { get; set; }
        private string DbName { get; set; }


        /// <summary>
        /// 初始化一个Json实体转为MySql的实例。注：数据库必须开放information_schema的读权限，否则会导致转换失败。
        /// </summary>
        /// <param name="dbServerIP">数据库IP地址</param>
        /// <param name="dbUserId">数据库用户名</param>
        /// <param name="dbPassword">数据库密码</param>
        /// <param name="dbName">数据库名</param>
        public JsonEntityToSql(string dbServerIP, string dbUserId, string dbPassword, string dbName)
        {
            DbName = dbName;
            Information_schemaConnectionString = $"server={dbServerIP}; port=3306;user id={dbUserId};password={dbPassword};persistsecurityinfo=True;database=information_schema";
        }

        /// <summary>
        /// 初始化一个Json实体转为MySql的实例。注：数据库必须开放information_schema的读权限，否则会导致转换失败。
        /// </summary>
        /// <param name="dbServerIP">数据库IP地址</param>
        /// <param name="dbUserId">数据库用户名</param>
        /// <param name="dbPassword">数据库密码</param>
        /// <param name="dbName">数据库名</param>
        /// <param name="dbPort">数据库端口</param>
        public JsonEntityToSql(string dbServerIP, string dbUserId, string dbPassword, string dbName, string dbPort)
        {
            DbName = dbName;
            Information_schemaConnectionString = $"server={dbServerIP}; port={dbPort};user id={dbUserId};password={dbPassword};persistsecurityinfo=True;database=information_schema";

        }

        public string Json2DeleteSql(string json)
        {
            var dic = json.ToJsonDictionary();
            var sqlString = new SqlString(dic)
            {
                MySqlConnection = new MySqlConnection(Information_schemaConnectionString),
                DataBaseName = DbName
            };
            return sqlString.GetDeleteSqls();
        }

        public string Json2InsertSql(string json)
        {
            var dic = json.ToJsonDictionary();
            var sqlString = new SqlString(dic)
            {
                MySqlConnection =new MySqlConnection(Information_schemaConnectionString),
                DataBaseName = DbName
            };
            return sqlString.GetInsertSqls();
        }

        public string Json2SelectSql(string json)
        {
            var dic = json.ToJsonDictionary();
            var sqlString = new SqlString(dic)
            {
                MySqlConnection = new MySqlConnection(Information_schemaConnectionString),
                DataBaseName = DbName
            };
            return sqlString.GetQuerySql();
        }

        public string Json2UpdateSql(string json)
        {
            var dic = json.ToJsonDictionary();
            var sqlString = new SqlString(dic)
            {
                MySqlConnection =new MySqlConnection(Information_schemaConnectionString),
                DataBaseName = DbName
            };
            return sqlString.GetUpdateSqls();
        }
    }
}
