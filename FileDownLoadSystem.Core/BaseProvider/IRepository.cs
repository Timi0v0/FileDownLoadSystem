using FileDownLoadSystem.Core.Enums;
using FileDownLoadSystem.Core.Extensions;
using FileDownLoadSystem.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FileDownLoadSystem.Core.BaseProvider
{
    /// <summary>
    ///仓储接口
    /// </summary>
    /// <typeparam name="TBaseModel"></typeparam>
    public interface IRepository<TBaseModel> where TBaseModel : BaseModel
    {
        public TBaseModel FindFirst(Expression<Func<TBaseModel, bool>> predicate);

        public IQueryable<TBaseModel> FindAsIQueryable(Expression<Func<TBaseModel, bool>> predicate);

        public TBaseModel FindFirst(Expression<Func<TBaseModel, bool>> predicate, Expression<Func<TBaseModel, Dictionary<object, QueryOrderBy>>> orderBy = null);

        public IQueryable<TBaseModel> FindAsIQueryable(Expression<Func<TBaseModel, bool>> predicate, Expression<Func<TBaseModel, Dictionary<object, QueryOrderBy>>> orderBy = null);
    }
}
