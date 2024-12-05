using FileDownLoadSystem.Core.EFDbContext;
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
    /// 定义抽象的仓储基类
    /// </summary>
    /// <typeparam name="TBaseModel">需要操作的实体类</typeparam>
    public abstract class RepositoryBase<TBaseModel> where TBaseModel : BaseModel
    {
        private readonly FileDownLoadSystemDbContext _defaultDBContext;
        /// <summary>
        /// 只允许读的权限
        /// </summary>
        public FileDownLoadSystemDbContext DbContext => _defaultDBContext;

        public RepositoryBase(FileDownLoadSystemDbContext dbContext)
        {
            _defaultDBContext = dbContext;
        }

        private DbSet<TBaseModel> _dbSet=> _defaultDBContext.Set<TBaseModel>();
        /// <summary>
        /// 根据条件（predicate）从数据库中查询第一个符合条件的记录
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual TBaseModel FindFirst(Expression<Func<TBaseModel, bool>> predicate) {
            return _dbSet.Where(predicate).FirstOrDefault();
        }
        /// <summary>
        /// 根据条件（predicate）返回一个 IQueryable 的结果集 允许延迟执行（延迟加载），使查询可以在之后继续追加筛选、排序等操作
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IQueryable<TBaseModel> FindAsIQueryable(Expression<Func<TBaseModel, bool>> predicate)
        {
            return _dbSet.Where(predicate);
        }
    }
}
