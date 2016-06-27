using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Json2Sql.ConditionModel
{
    /// <summary>
    /// 基础的实体与SQL条件语句(即WHERE)
    /// </summary>
    [DataContract]
    public class BaseCondition : Dictionary<string, object>, ICondition2Sql
    {
        [NonSerialized]
        private string m_ConditionName;
        public BaseCondition(string conditionName)
        {
            m_ConditionName = conditionName;
        }



        public string ToConditionSql()
        {
            //获取Dictionary
            switch (m_ConditionName)
            {
                case "Same":
            
                   

                    break;
                case "GreaterThan":
                    break;
                case "Different":
                    break;
                case "LessThan":
                    break;
                case "GreaterThanAndSame":
                    break;
                case "LessThanAndSame":
                    break;
                case "Between":
                    break;
                case "NotBetween":
                    break;
                case "Possible":
                    break;
                case "NotPossible":
                    break;
                case "In":
                    break;



            }
            return null;
        }
    }
}
