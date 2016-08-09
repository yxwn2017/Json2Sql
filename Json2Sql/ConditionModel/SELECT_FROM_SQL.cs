using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Json2Sql.ConditionModel
{
    public partial class SqlString
    {
        #region SELECT . FROM .
        /// <summary>
        /// 转换表的别名
        /// </summary>
        /// <param name="top">SELECT 区域的SQL</param>
        /// <param name="from">FROM 区域的SQL</param>
        /// <returns></returns>
        public Tuple<string, string> ConvertAsName(string top, string from)
        {
            string _top = "";
            string _from = "";

            #region SELECT区域的别名
            if (top.Contains(","))
            {

                var keyvlue = top.Split(',');
                foreach (var s in keyvlue)
                {
                    if (s.Contains("$"))
                    {
                        var alis = s.Remove(1, s.Split('$')[0].Length) + ",";
                        if (alis.IndexOf("(") >= 0 && alis.IndexOf(")") >= 0) //如果是函数，则移除掉``
                        {
                            alis = alis.Replace("`", "");
                        }
                        _top += alis;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(s.Trim()))
                        {
                            _top += s + ",";
                        }
                    }
                }
            }
            else
            {
                if (top.Contains("$"))
                {
                    _top += top.Remove(1, top.Split('$')[0].Length) + ",";
                }
                else
                {
                    _top = top;
                }

            }
            #endregion
            #region FROM区域的别名

            if (from.Contains("$"))
            {
                _from = from.Replace("$", "` AS `");
            }
            else
            {
                _from = from;
            }

            #endregion

            return new Tuple<string, string>(_top, _from);
        }

        /// <summary>
        /// 获取Entity节点的SQL语句
        /// </summary>
        /// <param name="name">传Entity及子级的Name</param>
        /// <param name="jsonSqlDictionary">Entity及子级节点</param>
        /// <returns>返回sql语句片段(分别为top部份、from部分、on连表限制条件部分)</returns>
        protected Tuple<string, string, string> GetEntitySql(Dictionary<string, object> jsonSqlDictionary, string name = "Entity")
        {

            var from = new StringBuilder();
            var top = new StringBuilder();
            var on = new StringBuilder();

            if (jsonSqlDictionary.Any())
            {
                var dic = jsonSqlDictionary;

                #region Entity
                if (name == Condition.Entity)
                {
                    if (dic != null)
                    {
                        foreach (var o1 in dic)
                        {
                            var tmpDic = o1.Value as Dictionary<string, object>;
                            if (tmpDic != null)
                            {
                                var result = GetEntitySql(tmpDic, o1.Key);
                                top.Append(result.Item1);
                                from.Append(result.Item2);
                                on.Append(result.Item3);
                            }
                        }
                    }
                }
                #endregion

                #region Main
                if (name == Condition.Main)
                {
                    //此处$转义dic
                    var newDic = EscapeDictionary(dic);
                    var topFrom = GetTopAndFromSql(newDic, "");
                    var topFromResult = ConvertAsName(topFrom.Item1, topFrom.Item2);

                    if (!string.IsNullOrEmpty(newDic.FirstOrDefault(m => m.Key.Contains("Block$") == true).Key))
                    {
                        top.Append(" * ");
                        from.Append(topFromResult.Item2);
                        var sss = "";

                    }
                    else
                    {
                        top.Append(topFromResult.Item1);
                    from.Append(topFromResult.Item2);
                    }


                }
                #endregion

                #region LinkInner
                if (name == Condition.Link)
                {
                    var newDic = EscapeDictionary(dic);
                    var topFrom = GetTopAndFromSql(newDic, Condition.InnerJoinOperator);
                    var topFromResult = ConvertAsName(topFrom.Item1, topFrom.Item2);
                    top.Append(topFromResult.Item1);
                    from.Append(topFromResult.Item2);
                }
                #endregion

                #region LinkLeft
                if (name == Condition.LinkLeft)
                {
                    var newDic = EscapeDictionary(dic);
                    var topFrom = GetTopAndFromSql(newDic, Condition.LeftJoinOperator);
                    var topFromResult = ConvertAsName(topFrom.Item1, topFrom.Item2);
                    top.Append(topFromResult.Item1);
                    from.Append(topFromResult.Item2);
                }
                #endregion

                #region LinkRight
                if (name == Condition.LinkRight)
                {
                    var newDic = EscapeDictionary(dic);
                    var topFrom = GetTopAndFromSql(newDic, Condition.RightJoinOperator);
                    var topFromResult = ConvertAsName(topFrom.Item1, topFrom.Item2);
                    top.Append(topFromResult.Item1);
                    from.Append(topFromResult.Item2);
                }
                #endregion

                #region LinkCondition
                if (name == Condition.LinkCondition)
                {
                    var onResult = GetOnSql(dic);
                    on.Append(onResult);
                }
                #endregion

            }

            return new Tuple<string, string, string>(top.ToString(), from.ToString(), on.ToString());

        }

        /// <summary>
        /// 取所有指定表的字段（表名选择器简写时，从数据库取所有字段）
        /// </summary>
        /// <param name="dicTable"></param>
        /// <returns></returns>
        private Dictionary<string, object> EscapeDictionary(Dictionary<string, object> dicTable)
        {
            var newDic = dicTable.ToDictionary(item => item.Key, item => item.Value);
            foreach (var item in dicTable)
            {
                var isShow = item.Value as bool?;
                if (isShow == true)
                {
                    //如果是仅变量，则退出
                    if (item.Key.IndexOf("$") == 0)
                    {
                        break;
                    }
                    var tableName = item.Key.IndexOf("$") > 0 ? item.Key.Split('$')[0] : item.Key;

                    var columnDic = new Dictionary<string, object>();
                    //从数据库获取所有列并设置键值对
                    if (MySqlConnection.State == ConnectionState.Closed)
                    {
                        MySqlConnection.Open();
                    }
                    var sqlCmd = new MySql.Data.MySqlClient.MySqlCommand
                    {
                        Connection = MySqlConnection,
                        //CommandText =
                        //    $"SELECT `COLUMN_NAME` FROM `COLUMNS` WHERE TABLE_SCHEMA='{DataBaseName}' AND TABLE_NAME='{tableName}';"
                        CommandText =
                            $"SELECT * FROM `{tableName}`  LIMIT 1;"
                    };
                    var dt = new DataTable();
                    dt.Load(sqlCmd.ExecuteReader());
                    foreach (DataColumn column in dt.Columns)
                    {
                        columnDic.Add(column.ToString(), true);
                    }
                    newDic[item.Key] = columnDic;
                }
            }

            return newDic;
        }

        protected virtual string GetOnSql(Dictionary<string, object> dic)
        {
            var on = new StringBuilder();
            if (dic != null && dic.Any())
            {
                foreach (var o1 in dic)
                {
                    var tmpDic1 = o1.Value as Dictionary<string, object>;
                    var onStr = GetSqlWhereOperator(tmpDic1, o1.Key);
                    on.Append(onStr);
                }
            }
            return on.ToString();
        }

        protected virtual Tuple<string, string> GetTopAndFromSql(Dictionary<string, object> dic, string operatroString)
        {
            var from = new StringBuilder();
            var top = new StringBuilder();


            if (dic != null)
            {
                if (dic.Any())
                {
                    #region 拼接TOP与FROM
                    foreach (var o2 in dic)
                    {
                        var columnList = o2.Value as Dictionary<string, object>;
                        //Regex.Match( o2.Key,"^Block($|\$).*?$").Value
                        //转小括号的SQL，可能包含选择语句，因此需要判断是否含有别名
                        if (o2.Key.IndexOf(Condition.BlockName + "$") >= 0 || o2.Key == Condition.BlockName)
                        {
                            from.Append(operatroString);
                            from.Append(" (");
                            var result = new Tuple<string, string>("", "");
                            //如果是子选择语句
                            if (columnList["Entity"] != null)
                            {
                                var sqlresult = this.GetQuerySql(columnList);
                                result = new Tuple<string, string>("", sqlresult);
                            }
                            else
                            {
                                result = GetTopAndFromSql(columnList, operatroString);
                            }

                            top.Append(result.Item1);
                            var fromResult = result.Item2;
                            fromResult = fromResult.Trim();
                            if (fromResult.IndexOf(operatroString.Trim()) == 0)
                            {
                                fromResult = fromResult.Remove(0, operatroString.Trim().Length);
                            }
                            from.Append(fromResult);
                            from.Append(")");
                            if (!string.IsNullOrEmpty(Regex.Match(o2.Key, "^Block\\$.*?$")?.Value))
                            {
                                from.Append($" AS {Regex.Match(o2.Key, "\\$.*?$")?.Value?.Replace("$", "")}");
                            }
                            continue;
                        }
                        if (o2.Key == Condition.Link)
                        {
                            var result = GetEntitySql(columnList, Condition.Link);
                            top.Append(result.Item1);
                            from.Append(result.Item2);

                            continue;
                        }
                        if (o2.Key == Condition.LinkLeft)
                        {
                            var result = GetEntitySql(columnList, Condition.LinkLeft);
                            top.Append(result.Item1);
                            from.Append(result.Item2);
                            continue;
                        }

                        if (o2.Key == Condition.LinkRight)
                        {
                            var result = GetEntitySql(columnList, Condition.LinkRight);
                            top.Append(result.Item1);
                            from.Append(result.Item2);
                            continue;
                        }

                        //from 子句拼接
                        string _fromStr = "";
                        if (o2.Key.IndexOf("$") != 0)
                            _fromStr = $" {operatroString} `{o2.Key}` ";


                        from.Append(_fromStr);


                        // top 列名子句拼接
                        if (columnList != null && columnList.Any())
                        {
                            foreach (var column in columnList)
                            {
                                bool? columnShow = column.Value as bool?;
                                if (columnShow == true)
                                {
                                    top.Append($"`{o2.Key}`.`{column.Key}`,");
                                }
                                if (columnShow == null)
                                {
                                    var columnName = column.Value as string;
                                    if (!string.IsNullOrEmpty(columnName))
                                    {
                                        top.Append($"`{o2.Key}`.`{column.Key}` AS `{columnName}`,");
                                    }
                                    else
                                    {
                                        //可能是嵌套的SQL语句
                                        if (column.Key == "Entity")
                                        {
                                            from.Remove(from.Length - _fromStr.Length, _fromStr.Length);
                                            //取SQL语句 
                                            var myNewDic = new Dictionary<string, object>();
                                            foreach (var o in columnList)
                                            {
                                                myNewDic.Add(o.Key, o.Value);
                                            }
                                            var stringSql = new SqlString(myNewDic);
                                            var resultSql = stringSql.GetQuerySql();
                                            from.Append($" {operatroString} ( {resultSql} ) AS `{o2.Key}` ");
                                            break;
                                        }
                                    }

                                }
                            }
                        }
                        else
                        {
                            var isFullColumns = o2.Value as bool?;
                            if (isFullColumns == true)
                            {
                                //if (MySqlConnection != null && !string.IsNullOrEmpty(DataBaseName))
                                //{
                                //}
                                //else
                                //{
                                //    top.Append(" *,");
                                //}
                                break;
                            }
                        }

                    }
                    #endregion
                }
            }
            //移除尾巴
            var topStr = top.ToString();
            var fromStr = from.ToString();
            return new Tuple<string, string>(topStr, fromStr);
        }



        #endregion
    }
}
