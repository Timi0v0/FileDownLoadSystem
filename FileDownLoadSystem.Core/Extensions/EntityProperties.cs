using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        /// <summary>
        /// 进行实体的验证，判断字典的key是否在实体中存在并且是否有效
        /// </summary>
        /// <param name="type"></param>
        /// <param name="dics"></param>
        /// <param name="properties"></param>
        /// <param name="removeNotContains"></param>
        /// <param name="removeKey"></param>
        /// <param name="ignoreFields"></param>
        /// <returns></returns>
        private static string ValidateDicInEntity(this Type type,Dictionary<string,object> dics, PropertyInfo[] properties,bool removeNotContains,bool removeKey, string[] ignoreFields)
        {
            //对字典进行验证
            if (dics == null || dics.Count == 0)
            {
                return "参数无效";
            }
            //检查需要校验的字段是否为空
            if (properties == null || properties.Length == 0)
            {
                //没有则从实体中获取
                properties = type.GetProperties().Where(x=>x.PropertyType.Name!= "List`1").ToArray();
            }
            //是否需要移除不包含的字段
            if (removeNotContains)
            {
                dics.Where(x => !properties.Any(p => p.PropertyType.Name == x.Key)).Select(x => x.Key).ToList().ForEach(
                     f =>
                     {
                         dics.Remove(f);
                     });
            }
            string keyName=type.GetKeyName();
            //是否需要移除主键
            if (removeKey)
            {
                dics.Remove(keyName);
            }
            //对字段进行验证
            foreach (var propertyInfo in properties)
            {
                //如果是主键对应的属性或者是可以忽略的属性 则不进行验证
                if (propertyInfo.Name==keyName||ignoreFields!=null&&ignoreFields.Contains(propertyInfo.Name))
                {
                    continue;
                }
                //判断属性是否为必填项 
                if (!dics.ContainsKey(propertyInfo.Name))
                {
                    if (propertyInfo.GetCustomAttributes(typeof(RequiredAttribute)).Count() > 0
                        && propertyInfo.PropertyType != typeof(int)
                        && propertyInfo.PropertyType != typeof(long)
                        && propertyInfo.PropertyType != typeof(byte)
                        && propertyInfo.PropertyType != typeof(decimal)
                        )
                    {
                        return $"{propertyInfo.Name}为必须提交项";
                    }
                    continue;
                }
            }
            return string.Empty;

        }
        public static string GetKeyName(this Type type)
        {
            return type.GetProperties().GetKeyName();
        }
        public static string GetKeyName(this PropertyInfo[] properties)
        {
            return properties.GetKeyName(false);
        }

        public static string GetKeyName(this PropertyInfo[] properties,bool keyType)
        {
            string keyName=string.Empty;
            foreach (var property in properties)
            {
                if (!property.IsKey())
                {
                    continue;
                }
                if (!keyType)
                {
                    return property.Name;
                }
                //从备注中获取主键对应的列的名称
                var attributes = property.GetCustomAttributes(typeof(ColumnAttribute), false);
                if (attributes.Length>0)
                {
                    return ((ColumnAttribute)attributes[0]).TypeName.ToLower();
                }
            }
            return keyName;
        }
    }
}
