using System.Collections.Generic;
using System.Linq;

namespace Json2Sql
{
    public static class DictionaryEx
    {

        public static object Find(this Dictionary<string, object> dictionary, string key)
        {
            object result = null;
            if (dictionary.Any())
            {
                if (dictionary.ContainsKey(key))
                {
                    return dictionary[key];
                }
                else
                {
                    foreach (var o in dictionary)
                    {
                        //=======================================================
                        var myDictionary = o.Value as Dictionary<string, object>;
                        if (myDictionary != null)
                        {
                            result = myDictionary.Find(key);
                            if (result != null) break;
                        }

                        //=======================================================
                        var myDictionaryList = o.Value as List<Dictionary<string, object>>;
                        if (myDictionaryList != null)
                        {
                            foreach (var _dictionary in myDictionaryList)
                            {
                                if (_dictionary != null)
                                {
                                    result = _dictionary.Find(key);
                                    if (result != null) return result;
                                }
                            }
                        }
                        //=======================================================
                    }


                }


            }
            return result;
        }
    }
}
