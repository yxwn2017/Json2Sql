using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Json2Sql.ModelEx;

namespace Json2Sql.ConditionModel
{
    public partial class SqlString
    {
        #region UPDATE SQL


        /// <summary>
        /// 将对像翻译成UPDATE SQL字符串
        /// </summary>
        /// <returns>返回SQL字符串</returns>
        public string GetUpdateSqls()
        {
            return GetUpdateSqls(SqlDictionary);
        }

        /// <summary>
        /// 将对像翻译成UPDATE SQL字符串(多条)
        /// </summary>
        /// <param name="dictionary">即将被翻译为将对像翻译成UPDATE SQL字符串的实体</param>
        /// <returns>返回SQL字符串</returns>
        public string GetUpdateSqls(Dictionary<string, object> dictionary)
        {
            var updateSqls = new StringBuilder();

            var jsonDictionary = dictionary;
            if (jsonDictionary != null && jsonDictionary.Any())
            {
                foreach (var o in jsonDictionary)
                {
                    var tbName = o.Key;
                    //获取多条记录的json对像
                    var updateRecords = o.Value as ArrayList;
                    if (updateRecords != null)
                    {

                        foreach (var insertRecord in updateRecords)
                        {
                            var insertSql = GetUpdateSql(tbName, (Dictionary<string, object>)insertRecord);
                            //多条
                            updateSqls.AppendLine(insertSql);
                        }
                    }
                    else
                    {
                        //如果不是数组，则获取单条
                        var insertRecord = o.Value as Dictionary<string, object>;
                        var insertSql = GetUpdateSql(tbName, insertRecord);
                        updateSqls.AppendLine(insertSql);
                    }

                }
            }
            return updateSqls.ToString();
        }

        /// <summary>
        /// 将对像翻译成UPDATE SQL字符串(单条)
        /// </summary>
        /// <param name="tbName">表名</param>
        /// <param name="updateRecord">即将被翻译为将对像翻译成UPDATE SQL字符串的数据实体</param>
        /// <returns>返回SQL字符串</returns>
        protected virtual string GetUpdateSql(string tbName, Dictionary<string, object> updateRecord)
        {
            if (!updateRecord.ContainsKey("id")) throw new Exception("未指定id，无法更新。");

            var updateSql = new StringBuilder();

            updateSql.Append($"UPDATE {tbName} SET ");

            var updateColumns = "";
            foreach (var o in updateRecord)
            {
                if (o.Key == "id") continue;
                updateColumns += $"`{o.Key}`=" + ((o.Value.GetType().Name == "String" || o.Value.GetType().Name == "DateTime") ? $"'{o.Value}'," : $"{o.Value},");
            }

            updateSql.Append(updateColumns.RemoveLastString(","));

            updateSql.Append(" WHERE ");

            updateSql.Append($" `id`='{updateRecord["id"]}';");

            return updateSql.ToString();
        }

        #endregion
    }
}
