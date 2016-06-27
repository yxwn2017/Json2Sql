using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EFunor.JianQu.Json2Sql.DataModel
{
    /// <summary>
    /// 数据列的引用信息
    /// </summary>
    public class ReferencedInfo
    {
        /// <summary>
        /// 数据库名
        /// </summary>
        public string DataBaseName { get; set; }
        /// <summary>
        /// 源表名
        /// </summary>
        public string SourceTableName { get; set; }
        /// <summary>
        /// 源列名
        /// </summary>
        public string SourceColumnsName { get; set; }
        /// <summary>
        /// 外键表名
        /// </summary>
        public string ForeignTableName { get; set; }
        /// <summary>
        /// 外键表的列名
        /// </summary>
        public string ForeignKeyName { get; set; }
    }
}
