using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Json2Sql
{
    public interface IJsonConditionSql
    {
        /*
 "子句":{ 
           
            "Same":{ 
                    "username":"waitaction",
                    "password":"123456",
                    "type":1
                  }   
        }
 =============================================================
        "Ampersand":{
        
            
            }
            */
        string SingleCondition2Sql(string json);


        //--操作符
        //相等 Same 
        //不等于 Different
        //大于 GreaterThan
        //小于 LessThan 
        //大于等于 GreaterThanAndSame
        //小于等于 LessThanAndSame
        //在某个范围内 Between
        //LIKE Possible

        //--运算符
        //AND Ampersand
        //OR  Maybe

    }
}
