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
        #region DELETE SQL

        /// <summary>
        /// 将对像翻译成DELETE SQL字符串
        /// </summary>
        /// <returns>返回SQL字符串</returns>
        public string GetDeleteSqls()
        {
            return GetDeleteSqls(SqlDictionary);
        }

        /// <summary>
        /// 将对像翻译成DELETE SQL字符串(多条)
        /// </summary>
        /// <param name="dictionary">即将被翻译为将对像翻译成DELETE SQL字符串的实体</param>
        /// <returns>返回SQL字符串</returns>
        public string GetDeleteSqls(Dictionary<string, object> dictionary)
        {
            var deleteSqls = new StringBuilder();

            var jsonDictionary = dictionary;
            if (jsonDictionary != null && jsonDictionary.Any())
            {
                foreach (var o in jsonDictionary)
                {
                    var tbName = o.Key;
                    //获取多条记录的json对像
                    var deleteRecords = o.Value as ArrayList;
                    if (deleteRecords != null)
                    {

                        foreach (var insertRecord in deleteRecords)
                        {
                            var insertSql = GetDeleteSql(tbName, (Dictionary<string, object>)insertRecord);
                            //多条
                            deleteSqls.AppendLine(insertSql);
                        }
                    }
                    else
                    {
                        //如果不是数组，则获取单条
                        var insertRecord = o.Value as Dictionary<string, object>;
                        var insertSql = GetDeleteSql(tbName, insertRecord);
                        deleteSqls.AppendLine(insertSql);
                    }

                }
            }
            return deleteSqls.ToString();
        }

        /// <summary>
        /// 将对像翻译成DELETE SQL字符串(单条)
        /// </summary>
        /// <param name="tbName">表名</param>
        /// <param name="deleteRecord">即将被翻译为将对像翻译成DELETE SQL字符串的数据键值对</param>
        /// <returns>返回SQL字符串</returns>
        protected virtual string GetDeleteSql(string tbName, Dictionary<string, object> deleteRecord)
        {
            if (!deleteRecord.ContainsKey("id")) throw new Exception("未指定id，无法删除。");
            var deleteSql = new StringBuilder();
            deleteSql.Append($"DELETE FROM {tbName} WHERE `id`='{deleteRecord["id"]}';");
            return deleteSql.ToString();
        }

        #endregion
    }
}
