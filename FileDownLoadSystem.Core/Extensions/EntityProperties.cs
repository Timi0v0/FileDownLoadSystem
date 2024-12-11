using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FileDownLoadSystem.Core.Extensions
{
    /// <summary>
    /// 属性扩展类
    /// </summary>
    public static class EntityProperties
    {
        /// <summary>
        /// 判断属性是否为主键
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        public static bool IsKey(this PropertyInfo propertyInfo)
        {
            var keyAttributes = propertyInfo.GetCustomAttributes(typeof(KeyAttribute), false);
            if (keyAttributes?.Length > 0 == true)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获取主键字段
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        public static PropertyInfo GetKeyProperty(this Type entity)
        {
            return entity.GetProperties().GetKeyProperty();
        }
        public static PropertyInfo GetKeyProperty(this PropertyInfo[] properties)
        {
            return properties.Where(c => c.IsKey()).FirstOrDefault();
        }

    }
}
