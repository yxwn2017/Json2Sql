using EFunor.JianQu.Json2Sql.DataModel;
using Json2KeyValue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Json2Sql.DataEngine
{
    /// <summary>
    /// 从数据取数据并将数据转为树状结构
    /// </summary>
    public class TreeDataHelper
    {
        /// <summary>
        /// 生成树状结构的数据
        /// </summary>
        /// <param name="dbResult">源数据(从数据库取回的列表字典型数据)</param>
        /// <param name="jsonObject">含配置信息的JSON串转换的对像</param>
        public static void GenerateTreeData(List<Dictionary<string, object>> data, string dataBaseConnectionString, JsonObject jsonObject)
        {
            var dbHelper = new DBHelper(dataBaseConnectionString);
            //树结构配置信息
            var foreReferenced = jsonObject?.FindChild("Config")?.GetValue<Dictionary<string, object>>("Referenced");
            var treeSchemas = jsonObject?.FindChild("Config")?.GetValue<Dictionary<string, object>>("TreeSchemas");

            if (foreReferenced != null)
            {
                if (data.Any())
                {
                    foreach (var dataItem in data)
                    {
                        //配置的引用外表的关系
                        foreach (var item in foreReferenced)
                        {
                            var isAlias = false;
                            var aliasName = "";
                            var foreKey = "";
                            if (item.Key.IndexOf("$") > 0) //判断是否有别名
                            {
                                isAlias = true;
                                aliasName = Regex.Match(item.Key, "\\$.*?$").Value.Replace("$", "");
                                foreKey = Regex.Match(item.Key, "^.*?\\$").Value.Replace("$", "");//配置信息的 源列名
                            }
                            else
                            {
                                isAlias = false;
                                foreKey = item.Key;//配置信息的 源列名
                            }

                            var foreValue = item.Value.ToString();//配置信息的 外键表名.外键列名

                            var columnReferenced = new ReferencedInfo();
                            columnReferenced.SourceColumnsName = foreKey;//源数据外键
                            columnReferenced.ForeignTableName = foreValue.Split('.')[0];//引用目标表名
                            columnReferenced.ForeignKeyName = foreValue.Split('.')[1];//引用目标列名

                            //拼接成取外键数据的SQL语句
                            var foreFeatchSql = $"select * from `{columnReferenced.ForeignTableName}` where `{columnReferenced.ForeignKeyName}`='{dataItem[foreKey].ToString()}' ";

                            //取外键数据

                            var foreDataResult = dbHelper.Query(foreFeatchSql);
                            //将返回来的外键数据值赋值给该字段
                            if (isAlias) //如果字段名有别名
                            {
                                FilterTreeData(treeSchemas, columnReferenced.ForeignTableName, ref foreDataResult);
                                dataItem.Add(aliasName, foreDataResult.Count == 1 ? (object)foreDataResult[0] : foreDataResult);
                                dataItem.Remove(foreKey);
                            }
                            else
                            {
                                FilterTreeData(treeSchemas, columnReferenced.ForeignTableName, ref foreDataResult);
                                dataItem[foreKey] = foreDataResult.Count == 1 ? (object)foreDataResult[0] : foreDataResult;
                            }
                        }

                    }

                }
            }
        }

        /// <summary>
        /// 生成树状结构的数据
        /// </summary>
        /// <param name="dbResult">源数据(从数据库取回的列表字典型数据)</param>
        /// <param name="json">含配置信息的JSON串</param>
        public static void GenerateTreeData(List<Dictionary<string, object>> data, string dataBaseConnectionString, string json)
        {
            var jsonObject = json.ToJsonObject();
            GenerateTreeData(data, dataBaseConnectionString, json);
        }

        /// <summary>
        ///  过滤掉 treeSchemas 不存在的数据列
        /// </summary>
        /// <param name="treeSchemas">数据的结构，可能会包含多层</param>
        /// <param name="tableName">表名</param>
        /// <param name="foreDataResult">从数据库取返回的列表字典数据</param>
        private static void FilterTreeData(Dictionary<string, object> treeSchemas, string tableName, ref List<Dictionary<string, object>> foreDataResult)
        {
            //如果含树结构，则按照树结构来返回数据,treeSchemas为JSON串里Cofig子节点下的配置的树结构信息
            if (treeSchemas != null && treeSchemas.ContainsKey(tableName))
            {
                var treeNodeSchema = treeSchemas[tableName] as Dictionary<string, object>;
                if (treeNodeSchema != null)
                {
                    foreach (var itemKv in foreDataResult)
                    {
                        var noIncludeColums = new List<string>();

                        foreach (var itemKvKeyvalue in itemKv)
                        {
                            if (!treeNodeSchema.ContainsKey(itemKvKeyvalue.Key))
                            {
                                noIncludeColums.Add(itemKvKeyvalue.Key);
                            }
                        }

                        foreach (var name in noIncludeColums)
                        {
                            itemKv.Remove(name);
                        }
                    }
                }
            }
        }
    }
}
