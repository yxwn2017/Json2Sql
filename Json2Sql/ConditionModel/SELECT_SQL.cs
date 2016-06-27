using System.Collections.Generic;
using System.Linq;
using System.Text;
using Json2Sql.ModelEx;

namespace Json2Sql.ConditionModel
{
    public partial class SqlString
    {
        #region SELECT SQL

        /// <summary>
        /// 将对像翻译成SELECT SQL字符串
        /// </summary>
        public string GetQuerySql()
        {
            return GetQuerySql(SqlDictionary);
        }

        /// <summary>
        /// 将对像翻译成SELECT SQL字符串
        /// </summary>
        /// <param name="dictionary">即将被翻译为SELECT SQL字符串的实体</param>
        /// <returns>返回SQL语句</returns>
        public string GetQuerySql(Dictionary<string, object> dictionary)
        {
            var selectSql = new StringBuilder();

            #region SELECT语句列名拼接
            //取SELECT 主体
            Dictionary<string, object> entity = null;
            if (dictionary.ContainsKey(Condition.Entity))
            {
                entity = dictionary[Condition.Entity] as Dictionary<string, object>;
            }

            Dictionary<string, object> condition = null;
            if (dictionary.ContainsKey(Condition.Condition_))
            {
                condition = dictionary[Condition.Condition_] as Dictionary<string, object>;
            }

            if (entity != null && entity.Any())
            {
                var result = GetEntitySql(entity);
                if (!string.IsNullOrEmpty(result.Item3.Trim()))
                {
                    selectSql.Append(
                        $"SELECT {result.Item1.RemoveLastString(",")} FROM {result.Item2} ON {result.Item3} ");
                }
                else
                {
                    selectSql.Append(
                        $"SELECT {result.Item1.RemoveLastString(",")} FROM {result.Item2}  ");
                }
            }
            #endregion

            #region WHERE 条件拼接
            if (condition != null)
            {
                selectSql.Append(" WHERE ");
                foreach (var o in condition)
                {
                    var conditionDic = o.Value as Dictionary<string, object>;
                    if (conditionDic != null)
                    {
                        selectSql.Append(GetSqlWhereOperator(conditionDic, o.Key));
                    }
                }
            }
            #endregion

            return selectSql.ToString();

        }

        #endregion
    }
}
