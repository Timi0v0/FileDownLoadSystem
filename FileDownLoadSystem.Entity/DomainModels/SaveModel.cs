using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileDownLoadSystem.Entity.DomainModels
{
    /// <summary>
    /// 用来存放前端传递的数据
    /// </summary>
    public class SaveModel
    {
        /// <summary>
        /// 主表数据
        /// </summary>
        public Dictionary<string, object> MainData { get; set; }
        /// <summary>
        /// 从表数据
        /// </summary>
        public List<Dictionary<string, object>> DetailData { get; set; }
        /// <summary>
        /// 存储需要删除的从表数据主键
        /// </summary>
        public List<object> DelKeys { get; set; }

        /// <summary>
        /// 从前台传入的其他参数(自定义扩展可以使用)
        /// </summary>
        public object Extra { get; set; }
    }
}
