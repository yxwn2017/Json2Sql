using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Json2Sql.ConditionModel
{
    /// <summary>
    /// 在什么之间
    /// </summary>
    public class Between : Dictionary<string,object>,IJsonConditionSql
    {
        public string SingleCondition2Sql(string json)
        {
            throw new NotImplementedException();
        }
    }
}
