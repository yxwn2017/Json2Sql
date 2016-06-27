using System.Collections.Generic;
using System.Linq;
using System.Text;
using Json2Sql.ModelEx;

namespace Json2Sql.ConditionModel
{
    public partial class SqlString
    {
        #region WHERE SQL

        /// <summary>
        /// 使用AND 或 OR 拼接SQL条件语句段
        /// </summary>
        /// <param name="dictionary">需要被转换为SQL条件语句段的实体 必须为And 或 Or的对像</param>
        /// <param name="linkConditionperator">连接字符 AND 或 OR </param>
        /// <returns>返回SQL条件语句段</returns>
        protected virtual string GetAndOrCondition(Dictionary<string, object> dictionary, string linkConditionperator)
        {
            var conditionStringBuilder = new StringBuilder();
            foreach (var o1 in dictionary)
            {
                var tmpDictinory = o1.Value as Dictionary<string, object>;
                if (tmpDictinory != null && tmpDictinory.Any())
                {
                    var str = GetSqlWhereOperator(tmpDictinory, o1.Key, linkConditionperator);
                    conditionStringBuilder.Append(str);
                    conditionStringBuilder.Append(linkConditionperator);
                }
            }
            return conditionStringBuilder.ToString().RemoveLastString(linkConditionperator);
        }

        /// <summary>
        /// 使用 （）括号拼接SQL条件语句段
        /// </summary>
        /// <param name="dictionary">需要被转换为SQL条件语句段的实体 必须为Block的对像</param>
        /// <param name="linkConditionperator">返回使用  () 拼接SQL条件语句段</param>
        /// <returns>返回SQL条件语句段</returns>
        protected virtual string GetBlockCondition(Dictionary<string, object> dictionary, string linkConditionperator)
        {
            var conditionStringBuilder = new StringBuilder();
            conditionStringBuilder.Append(" ( ");

            foreach (var o in dictionary)
            {
                var tmpDictinory = o.Value as Dictionary<string, object>;
                if (tmpDictinory != null && tmpDictinory.Any())
                {
                    var str = GetSqlWhereOperator(tmpDictinory, o.Key, linkConditionperator);
                    conditionStringBuilder.Append(str);
                    conditionStringBuilder.Append($" {linkConditionperator}");
                }
            }
            var result = conditionStringBuilder.ToString().RemoveLastString(linkConditionperator);

            return result + " ) ";
        }

        /// <summary>
        /// 使用 ORDER BY 拼接SQL条件语句段
        /// </summary>
        /// <param name="dictionary">需要被转换为SQL条件语句段的实体 必须为Order的对像</param>
        /// <returns>返回SQL条件语句段</returns>
        protected virtual string GetOrderByCondition(Dictionary<string, object> dictionary)
        {
            var conditionStringBuilder = new StringBuilder();
            conditionStringBuilder.Append(Condition.OrderOperator);
            foreach (var o in dictionary)
            {
                conditionStringBuilder.Append($" `{o.Key}` {o.Value},");
            }
            return conditionStringBuilder.ToString().RemoveLastString(",");
        }
        /// <summary>
        /// 使用 Limit 拼接SQL条件语句段
        /// </summary>
        /// <param name="dictionary">需要被转换为SQL条件语句段的实体 必须为Limit的对像</param>
        /// <returns>返回SQL条件语句段</returns>
        protected virtual string GetLimitCondition(Dictionary<string, object> dictionary)
        {
            var conditionStringBuilder = new StringBuilder();
            conditionStringBuilder.Append(Condition.LimitOperator);
            foreach (var o in dictionary)
            {
                conditionStringBuilder.Append($" {o.Value} ");
            }
            return conditionStringBuilder.ToString();
        }

        protected virtual string GetGroupOperator(Dictionary<string, object> dictionary)
        {
            var conditionStringBuilder = new StringBuilder();
            conditionStringBuilder.Append(Condition.GroupOperator);
            foreach (var o in dictionary)
            {
                conditionStringBuilder.Append($" {o.Value} ");
            }
            return conditionStringBuilder.ToString();

        }

        /// <summary>
        /// 使用  > = 等操作符拼接SQL条件语句侧面
        /// </summary>
        /// <param name="dictionary">需要被转换为SQL条件语句段的实体 必须为操作的对像</param>
        /// <param name="operatorName">操作符的英文名</param>
        /// <param name="andOr">AND或OR拼接</param>
        /// <returns>返回SQL条件语句段</returns>
        protected virtual string GetOperatorCondition(Dictionary<string, object> dictionary, string operatorName, string andOr = null)
        {
            var conditionStringBuilder = new StringBuilder();
            if (dictionary == null) return null;
            var operatorStr = GetConditionOperator(operatorName);
            foreach (var item in dictionary)
            {
                #region 条件操作符SQL拼接
                //优先处理一个键两个值的
                if (operatorName == Condition.BetweenName)
                {
                    var o = item.Value as Dictionary<string, object>;
                    if (o != null && o.Any())
                    {
                        foreach (var o1 in o)
                        {
                            var resultOperatorStr = string.Format(Condition.BetweenOperator, $"`{item.Key}`.`{o1.Key}`", $"'{o1.Value.ToString().Split('|')[0]}'", $"'{o1.Value.ToString().Split('|')[1]}'");
                            conditionStringBuilder.Append(resultOperatorStr);
                            conditionStringBuilder.Append($" {andOr}");
                        }
                    }
                }
                else if (operatorName == Condition.NotBetweenName)
                {
                    var o = item.Value as Dictionary<string, object>;
                    if (o != null && o.Any())
                    {
                        foreach (var o1 in o)
                        {
                            var resultOperatorStr = string.Format(Condition.NotBetweenOperator, $"`{item.Key}`.`{o1.Key}`", $"'{o1.Value.ToString().Split('|')[0]}'", $"'{o1.Value.ToString().Split('|')[1]}'");
                            conditionStringBuilder.Append(resultOperatorStr);
                            conditionStringBuilder.Append($" {andOr}");
                        }
                    }
                }
                else if (operatorName == Condition.SameName ||
                    operatorName == Condition.GreaterThanName ||
                    operatorName == Condition.DifferentName ||
                    operatorName == Condition.LessThanName ||
                    operatorName == Condition.GreaterThanAndSameName ||
                    operatorName == Condition.LessThanAndSameName ||
                    operatorName == Condition.PossibleName ||
                    operatorName == Condition.NotPossibleName ||
                    operatorName == Condition.InName ||
                    operatorName == Condition.IsName)
                {
                    var o = item.Value as Dictionary<string, object>;
                    if (o != null && o.Any())
                    {
                        foreach (var o1 in o)
                        {
                            //处理一个键一个值的
                            object strValue;
                            if (o1.Value.GetType().Name == "String" || o1.Value.GetType().Name == "DateTime")
                            {
                                if (o1.Value.ToString().IndexOf("$") != 0)
                                {
                                    strValue = $"'{o1.Value}'";
                                }
                                else
                                {
                                    strValue = $"{o1.Value}".Replace("$", "");
                                }
                            }
                            else
                            {
                                strValue = o1.Value;
                                var dic = strValue as Dictionary<string, object>;
                                if (dic != null)
                                {
                                    strValue = "(" + GetQuerySql(dic) + ")";


                                }
                            }

                            var resultOperatorStr = string.Format(operatorStr, $"`{item.Key}`.`{o1.Key}`", strValue);
                            conditionStringBuilder.Append(resultOperatorStr);
                            conditionStringBuilder.Append($" {andOr}");
                        }
                    }
                }
                #endregion

            }
            var result = conditionStringBuilder.ToString();
            result = result.RemoveLastString(andOr);
            return result;
        }

        /// <summary>
        /// 拼接WHERE条件语句部分sql
        /// </summary>
        /// <param name="operatorName">操作符的英文名称，例如=号的英文名为Equ</param>
        /// <param name="conditionKeyValueDictionary">列名与值的键值对</param>
        /// <param name="andOr">AND或OR</param>
        /// <returns>返回拼接好的完整Sql条件语句</returns>
        protected virtual string GetSqlWhereOperator(Dictionary<string, object> conditionKeyValueDictionary, string operatorName, string andOr = null)
        {

            var conditionStringBuilder = new StringBuilder();

            #region 拼接AND标识
            if (operatorName == Condition.AmpersandName)
            {
                return conditionStringBuilder.Append(GetAndOrCondition(conditionKeyValueDictionary, Condition.AmpersandOperator)).ToString();
            }
            #endregion

            #region 拼接OR标识
            if (operatorName == Condition.OrName)
            {
                return conditionStringBuilder.Append(GetAndOrCondition(conditionKeyValueDictionary, Condition.OrOperator)).ToString();
            }
            #endregion

            #region 拼接括号标识
            if (operatorName == Condition.BlockName)
            {
                return GetBlockCondition(conditionKeyValueDictionary, andOr);
            }
            #endregion

            #region Order标识拼接
            if (operatorName == Condition.OrderName)
            {
                return GetOrderByCondition(conditionKeyValueDictionary);
            }
            #endregion

            #region LIMIT标识拼接
            if (operatorName == Condition.LimitName)
            {
                return GetLimitCondition(conditionKeyValueDictionary);
            }

            #endregion

            #region GROUP标识拼接
            if (operatorName == Condition.GroupName)
            {
                return GetGroupOperator(conditionKeyValueDictionary);
            }

            #endregion


            #region 拼接操作符标识(如< = >)

            return GetOperatorCondition(conditionKeyValueDictionary, operatorName, andOr);

            #endregion



        }



        /// <summary>
        /// 返回操作符符号
        /// </summary>
        /// <param name="key">操作符英文名称</param>
        /// <returns>返回操作符符号</returns>
        protected virtual string GetConditionOperator(string key)
        {
            // ReSharper disable once RedundantAssignment
            var operatorString = key;
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
                case Condition.IsName:
                    operatorString = Condition.IsOperator;
                    break;
            }
            return operatorString;
        }

        #endregion
    }
}
