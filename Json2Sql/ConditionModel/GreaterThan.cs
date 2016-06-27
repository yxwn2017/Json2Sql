using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//前端JSON---->获取到最顶层的 运算符、括号、排序、实体数据、选择列
//                                                           ---->运算符---->解析条件操作符
//                                                                                    ---->解析参数和值---->拼接成单个操作符条件SQL
//                                                                                    ---->解析参数和值---->拼接成单个操作符条件SQL
//                                                                                                              ---->结合运算符再拼接各操作符的条件SQL
//                                                           ---->括号----->j
//
//
//
//
namespace Json2Sql.ConditionModel
{
    /// <summary>
    /// 大于
    /// </summary>
    public class GreaterThan : Dictionary<string,object> 
    {
      
    }
}
