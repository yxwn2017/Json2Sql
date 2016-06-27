using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Json2Sql
{
    /// <summary>
    /// 实体转化为条件sql语句
    /// </summary>
    public interface ICondition2Sql
    {
        string ToConditionSql();
    }
}
