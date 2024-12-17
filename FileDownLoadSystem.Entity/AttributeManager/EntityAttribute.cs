using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileDownLoadSystem.Entity.AttributeManager
{
    /// <summary>
    /// 实体属性 用来表面主表的一些信息以及其对应从表的信息
    /// </summary>
    public class EntityAttribute: Attribute
    {
        /// <summary>
        /// 真实表名(数据库表名，若没有填写默认实体为表名)
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// 表中文名
        /// </summary>
        public string TableCnName { get; set; }
        /// <summary>
        /// 子表
        /// </summary>
        public Type[] DetailTable { get; set; }
        /// <summary>
        /// 子表中文名
        /// </summary>
        public string DetailTableCnName { get; set; }
        /// <summary>
        /// 其在其他表种充当外键名称
        /// </summary>
        public string ForeignKeyName { get; set; }
    }
}
