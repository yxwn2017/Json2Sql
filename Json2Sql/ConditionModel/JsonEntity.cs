using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Json2Sql.ConditionModel
{
    /// <summary>
    /// json字符串转化为Dictionary的操作
    /// </summary>
    public static class JsonEntity
    {
    

        /// <summary>
        /// 将json字符串完全转化成Dictionary的树状结构
        /// </summary>
        /// <param name="json">json字符串</param>
        public static Dictionary<string, object> ToJsonDictionary(this string json)
        {
            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(json, JsonSerializerSettingsEx.DefaultSetting );
            return ToJsonDictionary(dictionary);
        }

        /// <summary>
        /// 将Dictionary完全转化成Dictionary的树状结构
        /// </summary>
        public static Dictionary<string, object> ToJsonDictionary(this Dictionary<string, object> dictionary)
        {
            if (dictionary != null && dictionary.Any())
            {

                for (var i = 0; i < dictionary.Count; i++)
                {
                    var oStr = dictionary.ElementAt(i).Value.ToString();

                    if (oStr.IndexOf("{") == 0)
                    {
                        var oEntity = JsonConvert.DeserializeObject<Dictionary<string, object>>(oStr, JsonSerializerSettingsEx.DefaultSetting);
                        dictionary[dictionary.ElementAt(i).Key] = ToJsonDictionary(oEntity);

                    }
                    if (oStr.IndexOf("[") == 0)
                    {
                        var oEntityList = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(oStr, JsonSerializerSettingsEx.DefaultSetting);

                        if (oEntityList.Any())
                        {
                            dictionary[dictionary.ElementAt(i).Key] = oEntityList;
                            for (int j = 0; j < oEntityList.Count; j++)
                            {
                                for (var k = 0; k < oEntityList[j].Count; k++)
                                {
                                    var oStr1 = oEntityList[j].ElementAt(k).Value.ToString();
                                    if (oStr1.IndexOf("{") == 0)
                                    {
                                        var oEntity1 = JsonConvert.DeserializeObject<Dictionary<string, object>>(oStr1, JsonSerializerSettingsEx.DefaultSetting);
                                        oEntityList[j][oEntityList[j].ElementAt(k).Key] = ToJsonDictionary(oEntity1);
                                    }
                                }
                            }
                        }
                    }
                }
            }


            return dictionary;

        }

    }


}
