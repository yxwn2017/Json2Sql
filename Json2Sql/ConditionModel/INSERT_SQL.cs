using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Json2Sql.ModelEx;

namespace Json2Sql.ConditionModel
{
    public partial class SqlString
    {
        #region INSERT SQL

        /// <summary>
        /// 将对像翻译成INSERT INTO SQL字符串
        /// </summary>
        /// <returns>返回SQL字符串</returns>
        public string GetInsertSqls()
        {
            return GetInsertSqls(SqlDictionary);
        }

        /// <summary>
        /// 将对像翻译成INSERT INTO SQL字符串(多条)
        /// </summary>
        /// <param name="dictionary">即将被翻译为INSERT INTO SQL字符串的实体</param>
        /// <returns>返回SQL字符串</returns>
        public string GetInsertSqls(Dictionary<string, object> dictionary)
        {
            var insertSqls = new StringBuilder();

            var jsonDictionary = dictionary;
            if (jsonDictionary != null && jsonDictionary.Any())
            {
                foreach (var o in jsonDictionary)
                {
                    var tbName = o.Key;
                    //获取多条记录的json对像

                    var insertRecords = o.Value as ArrayList;
                    if (insertRecords != null)
                    {

                        foreach (var insertRecord in insertRecords)
                        {
                            var insertSql = GetInsertSql(tbName, (Dictionary<string, object>)insertRecord);
                            //多条
                            insertSqls.AppendLine(insertSql);
                        }
                    }
                    else
                    {
                        //如果不是数组，则获取单条
                        var insertRecord = o.Value as Dictionary<string, object>;
                        var insertSql = GetInsertSql(tbName, insertRecord);
                        insertSqls.AppendLine(insertSql);
                    }
                }
            }
            return insertSqls.ToString();
        }


        /// <summary>
        /// 将对像翻译成INSERT INTO SQL字符串(单条)
        /// </summary>
        /// <param name="tbName">表名</param>
        /// <param name="insertRecord">实体</param>
        /// <returns>返回SQL字符串</returns>
        protected virtual string GetInsertSql(string tbName, Dictionary<string, object> insertRecord)
        {
            var insertSql = new StringBuilder();

            insertSql.Append($"INSERT INTO {tbName} (");

            var isContainId = false;
            var _key = "";
            foreach (var o in insertRecord)
            {
                if (o.Key == "id")
                {
                    isContainId = true;
                }
                _key = _key + $"`{o.Key}`,";
            }


            var _value = "";
            foreach (var o in insertRecord)
            {
                if (o.Key == "id")
                {
                    isContainId = true;
                }
                _value = _value + ((o.Value.GetType().Name == "String" || o.Value.GetType().Name == "DateTime") ? $"'{o.Value}'," : $"{o.Value},");
            }


            if (isContainId == false)
            {
                _key = _key + "`id`,";
                _value = _value + $"UUID(),";
            }

            _key = _key.RemoveLastString(",");
            _value = _value.RemoveLastString(",");

            insertSql.Append(_key);
            insertSql.Append(") VALUES (");
            insertSql.Append(_value);

            insertSql.Append(");");
            return insertSql.ToString();
        }



        #endregion
    }
}
