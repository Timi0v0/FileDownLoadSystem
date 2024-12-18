using FileDownLoadSystem.Core.Extensions;
using FileDownLoadSystem.Core.Utilities.Response;
using FileDownLoadSystem.Entity;
using FileDownLoadSystem.Entity.AttributeManager;
using FileDownLoadSystem.Entity.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FileDownLoadSystem.Core.BaseProvider
{
    public class BaseService<TBaseModel, TBaseRepository>
         where TBaseModel : BaseModel
        where TBaseRepository : IRepository<TBaseModel>
    {
        protected TBaseRepository Repository { get; set; }
        public BaseService(TBaseRepository baseRepository)
        {
            Repository = baseRepository;
        }

        public TBaseModel FindFirst()
        {
            return Repository.FindFirst(x=>x.Id==1);
        } 
        
        public virtual WebResponseContent AddEntity(TBaseModel baseModel)
        {
            return Add<TBaseModel>(baseModel,null);
        }
        /// <summary>
        /// 添加主表和明细表
        /// </summary>
        /// <typeparam name="TDetail">子表类型</typeparam>
        /// <param name="baseModel"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public WebResponseContent Add<TDetail>(TBaseModel baseModel,List<TDetail> list = null,bool validationEntity=true)
            where TDetail : class
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 从主表获取到对应的明细表的类型
        /// </summary>
        /// <returns></returns>
        protected virtual Type? GetRealDetailType()
        {
            return typeof(TBaseModel).GetCustomAttribute<EntityAttribute>()?.DetailTable[0];
        }
        /// <summary>
        /// 获取外键的属性
        /// </summary>
        /// <returns></returns>
        protected virtual string? GetForeignKeyProperty()
        {
            return typeof(TBaseModel).GetCustomAttribute<EntityAttribute>()?.ForeignKeyName;
        }
        /// <summary>
        /// 更新主表和明细表
        /// </summary>
        /// <typeparam name="TBaseModel"></typeparam>
        /// <param name="saveModel"></param>
        /// <returns></returns>
        public WebResponseContent? Update(SaveModel saveModel)
        {
            try
            {
                WebResponseContent webResponseContent = new WebResponseContent();
                //检查前端传入的数据是否为空
                if (saveModel == null)
                {
                    return webResponseContent.Error(Enums.ResponseType.ParametersLack);
                }
                //获取主表的主键
                PropertyInfo mainKeyProperty = typeof(TBaseModel).GetKeyProperty();
                //获取主键类型对应的默认值
                object mainKeyDefaultValue = mainKeyProperty.PropertyType.Assembly.CreateInstance(mainKeyProperty.PropertyType.FullName!)!;
                //判断主键是否为空 或者前端返回的数据不包含主键 或者主键的值为空 或者主键的值等于默认值
                if (mainKeyProperty == null || !saveModel.MainData.ContainsKey(mainKeyProperty.Name) || saveModel.MainData[mainKeyProperty.Name] == null
                    || saveModel.MainData[mainKeyProperty.Name]==mainKeyDefaultValue)
                {
                    return webResponseContent.Error(Enums.ResponseType.NoKey);
                }

                //如果没有明细数据 直接更新主表
                if (saveModel.DetailData == null || saveModel.DetailData.Count == 0)
                {
                    TBaseModel model=saveModel.MainData.DicToEntity<TBaseModel>();
                    Repository.Update(model);
                    return webResponseContent.OK(data:model);
                }
                //除去明细表里面的空列表
                saveModel.DetailData=saveModel.DetailData.Where(x=>x.Count()> 0).ToList();
                //从主表获取到对应的明细表的类型
                Type detailType = GetRealDetailType();
                //获取明细表的主键
                PropertyInfo detailKeyProperty = detailType.GetKeyProperty();
                //获取明细表的外键
                string detailForeignKey = GetForeignKeyProperty();
                //对明细表的主键和外键进行检查
                if (detailKeyProperty == null || detailForeignKey == null)
                {
                    return webResponseContent.Error(Enums.ResponseType.NoKey);
                }
                //获取明细表主键以及外键的默认值 
                object detailKeyDefaultValue = detailKeyProperty.PropertyType.Assembly.CreateInstance(detailKeyProperty.PropertyType.FullName!)!;
                foreach (Dictionary<string,object> dic in saveModel.DetailData)
                {
                    //如果数据中不包含主键 代表是新增数据
                    if (!dic.ContainsKey(detailKeyProperty.Name))
                    {
                        dic.Add(detailKeyProperty.Name, detailKeyDefaultValue);
                        if (dic.ContainsKey(detailForeignKey))
                        {
                            dic[detailForeignKey] = mainKeyDefaultValue;
                        }
                        else
                        {
                            dic.Add(detailForeignKey,mainKeyDefaultValue);
                        }
                        continue;
                    }
                    if (dic[detailKeyProperty.Name]==null)
                    {
                        return webResponseContent.Error(Enums.ResponseType.KeyError);
                    }
                    //如果数据中包含主键 代表是更新数据 检查其外键
                    if (!dic.ContainsKey(detailForeignKey) || dic[detailForeignKey] == null || dic[detailForeignKey]==mainKeyDefaultValue)
                    {
                        return webResponseContent.Error($"{detailForeignKey} 是必要的参数");
                    }

                }
                return this.GetType().GetMethod("UpdateToEntity")?.MakeGenericMethod(new Type[] {detailType }).Invoke(this, new object[] { saveModel, mainKeyProperty, detailKeyProperty,mainKeyDefaultValue }) as WebResponseContent;
            }
            catch (Exception ex)
            {
                return new WebResponseContent().Error(ex.ToString());
            }
        }
        /// <summary>
        /// 更新主表和明细表导数据库
        /// </summary>
        /// <typeparam name="TDetail"></typeparam>
        /// <param name="saveModel"></param>
        /// <param name="mainKeyProperty"></param>
        /// <param name="detailKeyInfo"></param>
        /// <param name="mainKeyDefaultVal">主表的主键默认值</param>
        /// <returns></returns>
        public WebResponseContent UpdateToEntity<TDetail>(SaveModel saveModel,PropertyInfo mainKeyProperty, PropertyInfo detailKeyInfo,object mainKeyDefaultVal) where TDetail : class
        {
            try
            {
                WebResponseContent webResponseContent = new WebResponseContent();
                //获取主表数据
                TBaseModel mainData=saveModel.MainData.DicToEntity<TBaseModel>();
                //获取从表的数据
                List<TDetail> detailData = saveModel.DetailData.DicToList<TDetail>();
                //新增对象
                List<TDetail> addList = new List<TDetail>();
                //修改对象
                List<TDetail> updateList = new List<TDetail>();

                //获取新增和修改的对象
                foreach (TDetail detail in detailData)
                {
                    //如果主键的值等于默认值 代表是新增数据
                    object detaiDefaultVal = detailKeyInfo.PropertyType.Assembly.CreateInstance(detailKeyInfo.PropertyType.FullName!)!;
                    if (detailKeyInfo.GetValue(detail)== detaiDefaultVal)
                    {
                        //新增数据需要将从表的外键赋值为主表的主键值
                        detailKeyInfo
                        addList.Add(detail);
                        continue;
                    }

                }

                return webResponseContent.OK();
            }
            catch (Exception ex)
            {
                return new WebResponseContent().Error(ex.ToString());
            }
        }

    }
}
