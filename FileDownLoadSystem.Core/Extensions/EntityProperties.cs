using FileDownLoadSystem.Core.Const;
using FileDownLoadSystem.Core.Utilities.Response;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
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
        private static string ValidateDicInEntity(this Type type, Dictionary<string, object> dics, PropertyInfo[] properties, bool removeNotContains, bool removeKey, string[] ignoreFields)
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
                properties = type.GetProperties().Where(x => x.PropertyType.Name != "List`1").ToArray();
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
            string keyName = type.GetKeyName();
            //是否需要移除主键
            if (removeKey)
            {
                dics.Remove(keyName);
            }
            //对字段进行验证
            foreach (var propertyInfo in properties)
            {
                //如果是主键对应的属性或者是可以忽略的属性 则不进行验证
                if (propertyInfo.Name == keyName || ignoreFields != null && ignoreFields.Contains(propertyInfo.Name))
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

        public static string GetKeyName(this PropertyInfo[] properties, bool keyType)
        {
            string keyName = string.Empty;
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
                if (attributes.Length > 0)
                {
                    return ((ColumnAttribute)attributes[0]).TypeName.ToLower();
                }
            }
            return keyName;
        }

        /// <summary>
        /// 验证集合的属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entityList"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static WebResponseContent ValidationEntityList<T>(this List<T> entityList, Expression<Func<T, object>> expression = null)
        {
            WebResponseContent responseData = new WebResponseContent();
            foreach (T entity in entityList)
            {
                responseData = entity.ValidationEntity(expression);
                if (!responseData.Status)
                {
                    return responseData;
                }
            }
            responseData.Status = true;
            return responseData;
        }
        /// <summary>
        /// 指定需要验证的字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="expression">对指定属性进行验证x=>{x.Name,x.Size}</param>
        /// <returns></returns>
        public static WebResponseContent ValidationEntity<T>(this T entity, Expression<Func<T, object>> expression = null, Expression<Func<T, object>> validateProperties = null)
        {
            return ValidationEntity<T>(entity, expression?.GetExpressionProperty<T>(), validateProperties?.GetExpressionProperty<T>());
        }
        /// <summary>
        /// specificProperties=null并且validateProperties=null，对所有属性验证，只验证其是否合法，不验证是否为空(除属性标识指定了不能为空外)
        /// specificProperties!=null，对指定属性校验，并且都必须有值
        /// null并且validateProperties!=null，对指定属性校验，不判断是否有值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="specificProperties">验证指定的属性，并且非空判断</param>
        /// <param name="validateProperties">验证指定属性，只对字段合法性判断，不验证是否为空</param>
        /// <returns></returns>
        public static WebResponseContent ValidationEntity<T>(this T entity, string[] specificProperties, string[] validateProperties = null)
        {
            WebResponseContent responseData = new WebResponseContent();
            if (entity == null) return responseData.Error("对象不能为null");

            PropertyInfo[] propertyArray = typeof(T).GetProperties();
            //若T为object取不到属性
            if (propertyArray.Length == 0)
            {
                propertyArray = entity.GetType().GetProperties();
            }
            List<PropertyInfo> compareProper = new List<PropertyInfo>();

            //只验证数据合法性，验证非空
            if (specificProperties != null && specificProperties.Length > 0)
            {
                compareProper.AddRange(propertyArray.Where(x => specificProperties.Contains(x.Name)));
            }

            //只验证数据合法性，不验证非空
            if (validateProperties != null && validateProperties.Length > 0)
            {
                compareProper.AddRange(propertyArray.Where(x => validateProperties.Contains(x.Name)));
            }
            if (compareProper.Count() > 0)
            {
                propertyArray = compareProper.ToArray();
            }
            foreach (PropertyInfo propertyInfo in propertyArray)
            {
                object value = propertyInfo.GetValue(entity);
                //设置默认状态的值
                if (propertyInfo.Name == "Enable" || propertyInfo.Name == "AuditStatus")
                {
                    if (value == null)
                    {
                        propertyInfo.SetValue(entity, 0);
                        continue;
                    }
                }
                //若存在specificProperties并且属性为数组specificProperties中的值，校验时就需要判断是否为空
                var reslut = propertyInfo.ValidationProperty(value,
                    specificProperties != null && specificProperties.Contains(propertyInfo.Name) ? true : false
                    );
                if (!reslut.Item1)
                    return responseData.Error(reslut.Item2);
            }
            return responseData.OK();
        }

        /// <summary>
        /// 验证每个属性的值是否正确
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <param name="objectVal">属性的值</param>
        /// <param name="required">是否指定当前属性必须有值</param>
        /// <returns></returns>
        public static (bool, string, object) ValidationProperty(this PropertyInfo propertyInfo, object objectVal, bool required)
        {
            if (propertyInfo.IsKey()) { return (true, null, objectVal); }

            string val = objectVal == null ? "" : objectVal.ToString().Trim();

            string requiredMsg = string.Empty;
            if (!required)
            {
                var reuireVal = propertyInfo.GetTypeCustomValues<RequiredAttribute>(x => new { x.AllowEmptyStrings, x.ErrorMessage });
                if (reuireVal != null && !Convert.ToBoolean(reuireVal["AllowEmptyStrings"]))
                {
                    required = true;
                    requiredMsg = reuireVal["ErrorMessage"];
                }
            }
            //如果不要求为必填项并且值为空，直接返回
            if (!required && string.IsNullOrEmpty(val))
                return (true, null, objectVal);

            if ((required && val == string.Empty))
            {
                if (requiredMsg != "") return (false, requiredMsg, objectVal);
                string propertyName = propertyInfo.GetTypeCustomValue<DisplayAttribute>(x => new { x.Name });
                return (false, requiredMsg + (string.IsNullOrEmpty(propertyName) ? propertyInfo.Name : propertyName) + "不能为空", objectVal);
            }
            //列名
            string typeName = propertyInfo.GetSqlDbType();

            //如果没有ColumnAttribute的需要单独再验证，下面只验证有属性的
            if (typeName == null) { return (true, null, objectVal); }
            //验证长度
            return typeName.ValidationVal(val, propertyInfo);
        }


        /// <summary>
        /// 验证数据库字段类型与值是否正确，
        /// </summary>
        /// <param name="dbType">数据库字段类型(如varchar,nvarchar,decimal,不要带后面长度如:varchar(50))</param>
        /// <param name="value">值</param>
        /// <param name="propertyInfo">要验证的类的属性，若不为null，则会判断字符串的长度是否正确</param>
        /// <returns>(bool, string, object)bool成否校验成功,string校验失败信息,object,当前校验的值</returns>
        public static (bool, string, object) ValidationVal(this string dbType, object value, PropertyInfo propertyInfo = null)
        {
           throw new NotImplementedException();
        }

        public static string GetDisplayName(this PropertyInfo property)
        {
            string displayName = property.GetTypeCustomValue<DisplayAttribute>(x => new { x.Name });
            if (string.IsNullOrEmpty(displayName))
            {
                return property.Name;
            }
            return displayName;
        }
        /// <summary>
        /// 获取数据库类型，不带长度，如varchar(100),只返回的varchar
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        public static string GetSqlDbType(this PropertyInfo propertyInfo)
        {
            string dbType = propertyInfo.GetTypeCustomValue<ColumnAttribute>(x => new { x.TypeName });

            if (string.IsNullOrEmpty(dbType))
            {
                return dbType;
            }
            dbType = dbType.ToLower();
            if (dbType.Contains(SqlDbTypeName.NVarChar))
            {
                dbType = SqlDbTypeName.NVarChar;
            }
            else if (dbType.Contains(SqlDbTypeName.VarChar))
            {
                dbType = SqlDbTypeName.VarChar;
            }
            else if (dbType.Contains(SqlDbTypeName.NChar))
            {
                dbType = SqlDbTypeName.NChar;
            }
            else if (dbType.Contains(SqlDbTypeName.Char))
            {
                dbType = SqlDbTypeName.Char;
            }

            return dbType;
        }
        /// <summary>
        /// 获取类的单个指定属性的值(只会返回第一个属性的值)
        /// </summary>
        /// <param name="member">当前类</param>
        /// <param name="type">指定的类</param>
        /// <param name="expression">指定属性的值 格式 Expression<Func<entityt, object>> exp = x => new { x.字段1, x.字段2 };</param>
        /// <returns></returns>
        public static string GetTypeCustomValue<TEntity>(this MemberInfo member, Expression<Func<TEntity, object>> expression)
        {
            var propertyKeyValues = member.GetTypeCustomValues(expression);
            if (propertyKeyValues == null || propertyKeyValues.Count == 0)
            {
                return null;
            }
            return propertyKeyValues.First().Value ?? "";
        }
        /// <summary>
        /// 获取类的多个指定属性的值
        /// </summary>
        /// <param name="member">当前类</param>
        /// <param name="type">指定的类</param>
        /// <param name="expression">指定属性的值 格式 Expression<Func<entityt, object>> exp = x => new { x.字段1, x.字段2 };</param>
        /// <returns>返回的是字段+value</returns>
        public static Dictionary<string, string> GetTypeCustomValues<TEntity>(this MemberInfo member, Expression<Func<TEntity, object>> expression)
        {
            var attr = member.GetTypeCustomAttributes(typeof(TEntity));
            if (attr == null)
            {
                return null;
            }

            string[] propertyName = expression.GetExpressionProperty();
            Dictionary<string, string> propertyKeyValues = new Dictionary<string, string>();

            foreach (PropertyInfo property in attr.GetType().GetProperties())
            {
                if (propertyName.Contains(property.Name))
                {
                    propertyKeyValues[property.Name] = (property.GetValue(attr) ?? string.Empty).ToString();
                }
            }
            return propertyKeyValues;
        }
        /// <summary>
        /// 获取属性的指定属性
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object GetTypeCustomAttributes(this MemberInfo member, Type type)
        {
            object[] obj = member.GetCustomAttributes(type, false);
            if (obj.Length == 0) return null;
            return obj[0];
        }
        /// <summary>
        /// 获取对象里指定成员名称
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="properties"> 格式 Expression<Func<entityt, object>> exp = x => new { x.字段1, x.字段2 };或x=>x.Name</param>
        /// <returns></returns>
        public static string[] GetExpressionProperty<TEntity>(this Expression<Func<TEntity, object>> properties)
        {
            if (properties == null)
                return new string[] { };
            if (properties.Body is NewExpression)
                return ((NewExpression)properties.Body).Members.Select(x => x.Name).ToArray();
            if (properties.Body is MemberExpression)
                return new string[] { ((MemberExpression)properties.Body).Member.Name };
            if (properties.Body is UnaryExpression)
                return new string[] { ((properties.Body as UnaryExpression).Operand as MemberExpression).Member.Name };
            throw new Exception("未实现的表达式");
        }
    }
}
