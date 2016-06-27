using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Json2Sql.ConditionModel
{
    /// <summary>
    /// 等于
    /// </summary>
    public class Same : Dictionary<string, object>
    {
        public Same(Dictionary<string, object> condition)
        {
            foreach (var item in condition)
            {
                Add(item.Key, item.Value);
            }
       
        }


    }

  
}
