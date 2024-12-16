using FileDownLoadSystem.Core.Utilities.Response;
using FileDownLoadSystem.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public WebResponseContent Add<TDetail>(TBaseModel baseModel,List<TDetail> list = null)
        {

        }
    }
}
