using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FileDownLoadSystem.Core.Extensions
{
    /// <summary>
    /// 基础属性方法扩展
    /// </summary>
    public static class ObjectExtension
    {
        /// <summary>
        /// 将字典集合转换为可枚举集合
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public static IEnumerable<T> DicToEnumerable<T>(this List<Dictionary<string, object>> dics) where T : class
        {
            foreach (var item in dics)
            {
                T instance=Activator.CreateInstance<T>();
                //对当前对象的属性进行赋值
                foreach (PropertyInfo property in instance.GetType().GetProperties(BindingFlags.GetProperty|BindingFlags.Public|BindingFlags.Instance))
                {
                    //如果当前字典包含当前属性
                    if (item.ContainsKey(property.Name))
                    {
                        //获取当前属性的值
                        object value = item[property.Name];
                        //如果当前属性的值不为空
                        if (value != null)
                        {
                            property.SetValue(instance, value.ChangeType(property.PropertyType), null);
                        }
                    }
                }
                yield return instance;
            }
        }
        /// <summary>
        /// 将字典集合转换为集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dics"></param>
        /// <returns></returns>
        public static List<T> DicToList<T>(this List<Dictionary<string, object>> dics) where T : class
        {
            return dics.DicToEnumerable<T>().ToList();
        }
        /// <summary>
        /// 将字典转换为实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dic"></param>
        /// <returns></returns>
        public static T DicToEntity<T>(this Dictionary<string, object> dic) where T : class
        {
          //这里这样实现的目的是尽量将逻辑放在一个函数里面去实现
          return new List<Dictionary<string, object>>(){ dic}.DicToList<T>()[0];
        }

        public static object? ChangeType(this object instance, Type type)
        {
            if (instance==null)
            {
                return null;
            }
            try
            {
                //检查是否属于GUID类型的值
                if (type == typeof(Guid) || type == typeof(Guid?))
                {
                    string value= instance.ToString()!;
                    if (string.IsNullOrEmpty(value))
                    {
                        return null;
                    }
                    return Guid.Parse(value);
                }
                //检查是否属于非泛型的
                if (!type.IsGenericType)
                {
                    return Convert.ChangeType(instance, type);
                }
                if (type.ToString() == "System.Nullable`1[System.Boolean]" || type.ToString() == "System.Boolean")
                {
                    if (instance.ToString() == "0")
                        return false;
                    return true;
                }
                Type genericTypeDefinition = type.GetGenericTypeDefinition();
                if (genericTypeDefinition == typeof(Nullable<>))
                {
                    return Convert.ChangeType(instance, Nullable.GetUnderlyingType(type)!);
                }
            }
            catch 
            {
                return null;
            }
            return null;
        }
    }
}
