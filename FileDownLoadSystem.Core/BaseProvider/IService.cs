using FileDownLoadSystem.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileDownLoadSystem.Core.BaseProvider
{
    public interface IService<TBaseModel,TBaseRepository> 
        where TBaseModel:BaseModel
        where TBaseRepository : IRepository<TBaseModel>
    {
        public TBaseModel FindFirst();
    }
}
