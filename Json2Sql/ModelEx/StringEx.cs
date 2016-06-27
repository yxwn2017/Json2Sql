
using System.Collections.Generic;
using Json2KeyValue;

namespace Json2Sql.ModelEx
{
    public static class StringEx
    {
        /// <summary>
        /// 移除指定的尾巴字符
        /// </summary>
        /// <param name="tail">尾巴字符串</param>
        /// <param name="str">源字符串</param>
        /// <returns>返回已移除的最终字符串</returns>
        public static string RemoveLastString(this string str, string tail)
        {

            if (string.IsNullOrEmpty(str) || string.IsNullOrEmpty(tail)) return str;
            var _result = str;
            if (_result.Substring(_result.Length - tail.Length, tail.Length) == tail)
            {
                _result = _result.Remove(_result.Length - tail.Length, tail.Length);
            }
            return _result;
        }


        /// <summary>
        /// 将json字符串完全转化成Dictionary的树状结构
        /// </summary>
        /// <param name="json">json字符串</param>
        public static Dictionary<string, object> ToJsonDictionary(this string json)
        {
            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(json, JsonSerializerSettingsEx.DefaultSetting);
            //return dictionary.ToJsonDictionary();
            return dictionary;
        }
    }
}
