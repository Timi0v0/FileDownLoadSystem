using FileDownLoadSystem.Core.Extensions;
using FileDownLoadSystem.Core.Utilities.Response;
using FileDownLoadSystem.Entity;
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

        public WebResponseContent Update<TbaseModel>(SaveModel saveModel)
            where TbaseModel : BaseModel
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
                PropertyInfo mainKeyProperty = typeof(TbaseModel).GetKeyProperty();
                //判断主键是否为空 或者前端返回的数据不包含主键 或者主键的值为空
                if (mainKeyProperty == null || !saveModel.MainData.ContainsKey(mainKeyProperty.Name) || saveModel.MainData[mainKeyProperty.Name] == null)
                {
                    return webResponseContent.Error(Enums.ResponseType.NoKey);
                }
                //如果没有明细数据 直接更新主表
                if (saveModel.DetailData == null || saveModel.DetailData.Count == 0)
                {
                    TbaseModel model=saveModel.MainData.DicToEntity<TbaseModel>();
                    Repository.Update(model);
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
