using System.Collections.Generic;

namespace Json2Sql.ConditionModel
{
    /// <summary>
    /// 将对像翻译为SQL语句
    /// </summary>
    public partial class SqlString
    {
        #region 初始及设置
        public MySql.Data.MySqlClient.MySqlConnection MySqlConnection { get; set; }
        //public string DataBaseName { get; set; }
        public Dictionary<string, object> SqlDictionary { get; set; }
        public SqlString(Dictionary<string, object> sqlDictionary)
        {
            SqlDictionary = sqlDictionary;
        }
        #endregion
    }
}
