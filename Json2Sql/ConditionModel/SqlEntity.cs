using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Json2Sql.ConditionModel
{
    public static class SqlEntity
    {
        /// <summary>
        /// Json最顶层的对像拼接成完整的查询sql
        /// </summary>
        public static string GetQuerySql(this Dictionary<string, object> conditionKeyValueDictionary)
        {
            var conditionStringBuilder = new StringBuilder();

            #region SELECT语句列名拼接

            conditionStringBuilder.Append(" SELECT ");
            //拼接select语句
            var inerJoinDic = conditionKeyValueDictionary.Find("I") as Dictionary<string, object>;

            //内联接
            if (inerJoinDic != null && inerJoinDic.Any())
            {
                var from = new StringBuilder();
                var top = new StringBuilder();
                //获取表对像
                foreach (var o in inerJoinDic)
                {
                    from.Append($"`{o.Key}`,");
                    //对每张表对像解析
                    var keyValuePairs = o.Value as Dictionary<string, object>;
                    if (keyValuePairs != null)
                    {
                        foreach (var o1 in keyValuePairs)
                        {
                            if ((bool)o1.Value)
                            {
                                top.Append($" `{o.Key}.{o1.Key}`,");
                            }
                        }
                    }
                    else
                    {
                        var isFullColumns = o.Value as bool?;
                        if (isFullColumns != null && isFullColumns == true)
                        {
                            top.Append(" *,");
                        }
                    }
                }

                conditionStringBuilder.Append(top.ToString().RemoveLastString(","));

                conditionStringBuilder.Append($" FROM {from.ToString().RemoveLastString(",")} WHERE ");


            }
            #endregion

            #region AND拼接
            var ampersand =
                conditionKeyValueDictionary.FirstOrDefault(s => s.Key == Condition.AmpersandName).Value as Dictionary<string, object>;
            if (ampersand != null)
            {
                var result = GetSqlWhereOperator(ampersand, Condition.AmpersandName);
                conditionStringBuilder.Append(result);
            }
            #endregion

            #region OR拼接
            var or =
               conditionKeyValueDictionary.FirstOrDefault(s => s.Key == Condition.OrName).Value as Dictionary<string, object>;
            if (or != null)
            {
                var result = GetSqlWhereOperator(or, Condition.OrName);
                conditionStringBuilder.Append(result);
            }
            #endregion

            #region ORDERBY拼接
            var order =
              conditionKeyValueDictionary.FirstOrDefault(s => s.Key == Condition.OrderName).Value as Dictionary<string, object>;
            if (order != null)
            {
                var result = GetSqlWhereOperator(order, Condition.OrderName);
                conditionStringBuilder.Append(result);
            }
            #endregion

            #region LIMIT拼接
            var limit = conditionKeyValueDictionary.FirstOrDefault(s => s.Key == Condition.LimitName).Value as Dictionary<string, object>;
            if (limit != null)
            {
                var result = GetSqlWhereOperator(limit, Condition.LimitName);
                conditionStringBuilder.Append(result);
            }
            #endregion
            return conditionStringBuilder.ToString();
        }


        public static string GetInsertSqls(this Dictionary<string, object> dictionary)
        {
            var insertSqls = new StringBuilder();

            var jsonDictionary = dictionary.ToJsonDictionary();
            if (jsonDictionary != null && jsonDictionary.Any())
            {
                foreach (var o in jsonDictionary)
                {
                    var tbName = o.Key;
                    //获取多条记录的json对像
                    var insertRecords = o.Value as List<Dictionary<string, object>>;
                    if (insertRecords != null)
                    {

                        foreach (var insertRecord in insertRecords)
                        {
                            var insertSql = GetInsertSql(tbName, insertRecord);
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
        /// 获取插入记录的Sql语句
        /// </summary>
        public static string GetInsertSql(string tbName, Dictionary<string, object> insertRecord)
        {
            var insertSql = new StringBuilder();

            insertSql.Append($"INSERT INTO {tbName} (");

            insertSql.Append(
                insertRecord.Aggregate("", (current, o1) => current + $"`{o1.Key}`,")
                .RemoveLastString(","));

            insertSql.Append(") VALUES (");

            insertSql.Append(
                insertRecord.Aggregate("", (current, o1) => current + (o1.Value.GetType().Name == "String" ? $"'{o1.Value}'," : $"{o1.Value},"))
                    .RemoveLastString(","));

            insertSql.Append(");");
            return insertSql.ToString();
        }

        /// <summary>
        /// 获取更新记录的Sql语句
        /// </summary>

        public static string GetUpdateSqls(this Dictionary<string, object> dictionary)
        {
            var updateSqls = new StringBuilder();

            var jsonDictionary = dictionary.ToJsonDictionary();
            if (jsonDictionary != null && jsonDictionary.Any())
            {
                foreach (var o in jsonDictionary)
                {
                    var tbName = o.Key;
                    //获取多条记录的json对像
                    var updateRecords = o.Value as List<Dictionary<string, object>>;
                    if (updateRecords != null)
                    {

                        foreach (var insertRecord in updateRecords)
                        {
                            var insertSql = GetUpdateSql(tbName, insertRecord);
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



        public static string GetUpdateSql(string tbName, Dictionary<string, object> updateRecord)
        {
            if (!updateRecord.ContainsKey("id")) throw new Exception("未指定id，无法更新。");

            var updateSql = new StringBuilder();

            updateSql.Append($"UPDATE {tbName} SET ");

            var updateColumns = "";
            foreach (var o in updateRecord)
            {
                if (o.Key == "id") continue;
                updateColumns += $"`{o.Key}`=" + (o.Value.GetType().Name == "String" ? $"'{o.Value}'," : $"{o.Value},");
            }

            updateSql.Append(updateColumns.RemoveLastString(","));

            updateSql.Append(" WHERE ");

            updateSql.Append($" `id`='{updateRecord["id"]}';");

            return updateSql.ToString();
        }





        /// <summary>
        /// 获取删除记录的sql语句(一条或多条)
        /// </summary>
        public static string GetDeleteSqls(this Dictionary<string, object> dictionary)
        {
            var deleteSqls = new StringBuilder();

            var jsonDictionary = dictionary.ToJsonDictionary();
            if (jsonDictionary != null && jsonDictionary.Any())
            {
                foreach (var o in jsonDictionary)
                {
                    var tbName = o.Key;
                    //获取多条记录的json对像
                    var deleteRecords = o.Value as List<Dictionary<string, object>>;
                    if (deleteRecords != null)
                    {

                        foreach (var insertRecord in deleteRecords)
                        {
                            var insertSql = GetDeleteSql(tbName, insertRecord);
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
        /// 获取用于删除记录的sql语句
        /// </summary>
        public static string GetDeleteSql(string tbName, Dictionary<string, object> deleteRecord)
        {
            if (!deleteRecord.ContainsKey("id")) throw new Exception("未指定id，无法删除。");

            var deleteSql = new StringBuilder();

            deleteSql.Append($"DELETE FROM {tbName} WHERE `id`='{deleteRecord["id"]}';");

            return deleteSql.ToString();
        }
        /// <summary>
        /// 拼接WHERE条件语句部分sql
        /// </summary>
        /// <param name="operatorName">操作符的英文名称，例如=号的英文名为Same</param>
        /// <param name="conditionKeyValueDictionary">列名与值的键值对</param>
        /// <param name="andOr">AND或OR</param>
        /// <returns>返回拼接好的Sql条件语句</returns>
        public static string GetSqlWhereOperator(this Dictionary<string, object> conditionKeyValueDictionary, string operatorName, string andOr = null)
        {

            var conditionStringBuilder = new StringBuilder();

            #region 拼接AND标识
            if (operatorName == Condition.AmpersandName)
            {
                foreach (var o1 in conditionKeyValueDictionary)
                {
                    var tmpDictinory = o1.Value as Dictionary<string, object>;
                    if (tmpDictinory != null && tmpDictinory.Any())
                    {
                        var andOperatorStr = GetSqlWhereOperator(tmpDictinory, o1.Key, Condition.AmpersandOperator);
                        conditionStringBuilder.Append(andOperatorStr);
                        conditionStringBuilder.Append(Condition.AmpersandOperator);
                    }
                }
                var andResult = RemoveLastString(conditionStringBuilder.ToString(), Condition.AmpersandOperator);
                return andResult;
            }
            #endregion

            #region 拼接OR标识
            if (operatorName == Condition.OrName)
            {

                foreach (var o1 in conditionKeyValueDictionary)
                {
                    var tmpDictinory = o1.Value as Dictionary<string, object>;
                    if (tmpDictinory != null && tmpDictinory.Any())
                    {
                        var orOperatorStr = GetSqlWhereOperator(tmpDictinory, o1.Key, Condition.OrOperator);
                        conditionStringBuilder.Append(orOperatorStr);
                        conditionStringBuilder.Append(Condition.OrOperator);
                    }
                }
                var orResult = RemoveLastString(conditionStringBuilder.ToString(), Condition.OrOperator);
                return orResult;
            }
            #endregion

            #region 拼接括号标识
            //括号标识，再递归调用一次
            if (operatorName == Condition.BlockName)
            {
                conditionStringBuilder.Append(" ( ");

                foreach (var o in conditionKeyValueDictionary)
                {
                    conditionStringBuilder.Append(GetSqlWhereOperator(o.Value as Dictionary<string, object>, o.Key, andOr));
                    conditionStringBuilder.Append($" {andOr}");
                }

                string _result = RemoveLastString(conditionStringBuilder.ToString(), andOr);
                _result += " ) ";

                return _result;
            }
            #endregion

            #region Order标识拼接
            if (operatorName == Condition.OrderName)
            {
                conditionStringBuilder.Append(Condition.OrderOperator);
                foreach (var o in conditionKeyValueDictionary)
                {
                    conditionStringBuilder.Append($" `{o.Key}` {o.Value},");
                }
                string _result = RemoveLastString(conditionStringBuilder.ToString(), ",");
                return _result;
            }
            #endregion

            #region LIMIT标识拼接
            if (operatorName == Condition.LimitName)
            { 
                conditionStringBuilder.Append(Condition.LimitOperator);
                foreach (var o in conditionKeyValueDictionary)
                {
                    conditionStringBuilder.Append($" {o.Value} ");
                }
                string _result = conditionStringBuilder.ToString();
                return _result;
            }

            #endregion

            #region 拼接操作符标识(如< = >)
            if (conditionKeyValueDictionary == null) return null;
            var operatorStr = GetConditionOperator(operatorName);
            foreach (var item in conditionKeyValueDictionary)
            {
                //优先处理一个键两个值的
                if (operatorName == Condition.BetweenName)
                {
                    var resultOperatorStr = string.Format(Condition.BetweenOperator, $"{item.Key}", $"'{item.Value.ToString().Split('|')[0]}'", $"'{item.Value.ToString().Split('|')[1]}'");
                    conditionStringBuilder.Append(resultOperatorStr);
                    conditionStringBuilder.Append($" {andOr}");
                }
                else if (operatorName == Condition.NotBetweenName)
                {
                    var resultOperatorStr = string.Format(Condition.NotBetweenOperator, $"{item.Key}", $"'{item.Value.ToString().Split('|')[0]}'", $"'{item.Value.ToString().Split('|')[1]}'");
                    conditionStringBuilder.Append(resultOperatorStr);
                    conditionStringBuilder.Append($" {andOr}");
                }
                else
                {
                    //处理一个键一个值的
                    var resultOperatorStr = string.Format(operatorStr, $"{item.Key}", item.Value.GetType().Name == "String" ? $"'{item.Value}'" : item.Value);
                    conditionStringBuilder.Append(resultOperatorStr);
                    conditionStringBuilder.Append($" {andOr}");
                }

            }
            var result = conditionStringBuilder.ToString();
            result = RemoveLastString(result, andOr);
            return result;
            #endregion

        }


        /// <summary>
        /// 移除指定的尾巴字符
        /// </summary>
        /// <param name="tail">尾巴字符串</param>
        /// <param name="str">源字符串</param>
        /// <returns>返回已移除的最终字符串</returns>
        public static string RemoveLastString(this string str, string tail)
        {

            if (string.IsNullOrEmpty(str) || string.IsNullOrEmpty(tail)) return str;
            var _result = str;
            if (_result.Substring(_result.Length - tail.Length, tail.Length) == tail)
            {
                _result = _result.Remove(_result.Length - tail.Length, tail.Length);
            }

            return _result;
        }

        /// <summary>
        /// 返回操作符符号
        /// </summary>
        /// <param name="key">操作符英文名称</param>
        /// <returns>返回操作符符号</returns>
        public static string GetConditionOperator(this string key)
        {
            // ReSharper disable once RedundantAssignment
            string operatorString = key;
            switch (key)
            {
                case Condition.BetweenName:
                    operatorString = Condition.BetweenOperator;
                    break;
                case Condition.DifferentName:
                    operatorString = Condition.DifferentOperator;
                    break;
                case Condition.GreaterThanAndSameName:
                    operatorString = Condition.GreaterThanAndSameOperator;
                    break;
                case Condition.GreaterThanName:
                    operatorString = Condition.GreaterThanOperator;
                    break;
                case Condition.InName:
                    operatorString = Condition.InOperator;
                    break;
                case Condition.LessThanAndSameName:
                    operatorString = Condition.LessThanAndSameOperator;
                    break;
                case Condition.LessThanName:
                    operatorString = Condition.LessThanOperator;
                    break;
                case Condition.NotBetweenName:
                    operatorString = Condition.NotBetweenOperator;
                    break;
                case Condition.NotPossibleName:
                    operatorString = Condition.NotPossibleOperator;
                    break;
                case Condition.PossibleName:
                    operatorString = Condition.PossibleOperator;
                    break;
                case Condition.SameName:
                    operatorString = Condition.SameOperator;
                    break;
                default:

                    break;
            }
            return operatorString;
        }
    }
}
