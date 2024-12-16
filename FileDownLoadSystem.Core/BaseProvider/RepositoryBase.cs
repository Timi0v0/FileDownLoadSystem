using FileDownLoadSystem.Core.EFDbContext;
using FileDownLoadSystem.Core.Enums;
using FileDownLoadSystem.Core.Extensions;
using FileDownLoadSystem.Core.Utilities.Response;
using FileDownLoadSystem.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

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
            return _dbSet.Where(predicate).FirstOrDefault()!;
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


        public virtual TBaseModel FindFirst(Expression<Func<TBaseModel, bool>> predicate, Expression<Func<TBaseModel,Dictionary<object, QueryOrderBy>>> orderBy=null)
        {
            return FindAsIQueryable(predicate, orderBy).FirstOrDefault()!;
        }

        public IQueryable<TBaseModel> FindAsIQueryable(Expression<Func<TBaseModel, bool>> predicate, Expression<Func<TBaseModel, Dictionary<object, QueryOrderBy>>> orderBy = null)
        {
            if (orderBy==null)
            {
                return DbContext.Set<TBaseModel>().Where(predicate).GetIQueryableOrderBy(orderBy.GetExpressionToDic());
            }
            return DbContext.Set<TBaseModel>().Where(predicate);
        }
        /// <summary>
        /// 将实体添加到数据库中
        /// </summary>
        /// <param name="baseModels">实体对象列表</param>
        /// <param name="isSave">是否提交到数据库</param>
        public virtual void AddRange(IEnumerable<TBaseModel> baseModels,bool isSave = false)
        {
            _dbSet.AddRange(baseModels);
            if (isSave)
            {
                DbContext.SaveChanges();
            }
        }
        /// <summary>
        /// 将某个数据添加到数据库中
        /// </summary>
        /// <param name="baseModel">实体对象</param>
        /// <param name="isSave">是否提交到数据库</param>
        public virtual void Add(TBaseModel baseModel, bool isSave = false)
        {
            AddRange(new List<TBaseModel> { baseModel }, isSave);
        }
        /// <summary>
        /// 异步将某些数据添加到数据库中
        /// </summary>
        /// <param name="baseModel"></param>
        /// <returns></returns>
        public virtual Task AddRangeAsync(IEnumerable<TBaseModel> baseModels)
        {
           return _dbSet.AddRangeAsync(baseModels);
        }
        /// <summary>
        /// 异步将某个数据添加到数据库中
        /// </summary>
        /// <param name="baseModel"></param>
        /// <returns></returns>
        public virtual Task AddAsync(TBaseModel baseModel)
        {
            return AddRangeAsync(new List<TBaseModel>() { baseModel});
        }
        /// <summary>
        /// 删除某个数据
        /// </summary>
        /// <param name="baseModel"></param>
        /// <param name="isSave"></param>
        public virtual void Delete(TBaseModel baseModel, bool isSave = false)
        {
            _dbSet.Remove(baseModel);
            if (isSave)
            {
                DbContext.SaveChanges();
            }
        }
        /// <summary>
        /// 批量删除某些数据
        /// </summary>
        /// <param name="baseModels"></param>
        /// <param name="isSave"></param>
        public virtual void DeleteRange(IEnumerable<TBaseModel> baseModels, bool isSave = false)
        {
            _dbSet.RemoveRange(baseModels);
            if (isSave)
            {
                DbContext.SaveChanges();
            }
        }
        /// <summary>
        /// 根据条件删除某些数据 通过此方法进行删除性能比较低 执行原生态的SQL语句效率会更高
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="isSave"></param>
        public virtual void DeleteRange(Expression<Func<TBaseModel, bool>> predicate, bool isSave = false)
        {
            //dbContext.Database.ExecuteSqlRaw("DELETE FROM Entities WHERE Property = {0}", value);
            var baseModels = FindAsIQueryable(predicate).ToList();
            DeleteRange(baseModels, isSave);
        }
        /// <summary>
        /// 通过主键批量删除
        /// </summary>
        /// <param name="keys">主键key</param>
        /// <param name="delList">是否连明细一起删除</param>
        public virtual void DeleteWithKeys(object[] keys, bool delList = false)
        {
            //对传入的参数进行判断
            if (keys == null || keys.Length == 0)
            {
                return;
            }

        }

        /// <summary>
        /// 更新数据库
        /// </summary>
        /// <param name="baseModel"></param>

        public virtual int Update(TBaseModel baseModel)
        {
            _dbSet.Update(baseModel);
            return DbContext.SaveChanges();
        }
        /// <summary>
        /// 开放其他实体类去使用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="baseModel"></param>
        /// <returns></returns>
        public virtual int Update<T>(T baseModel) where T : class
        {
            DbContext.Set<T>().Update(baseModel);
            return DbContext.SaveChanges();
        }
        /// <summary>
        /// 保存更改
        /// </summary>
        public virtual void SaveChanges()
        {
            DbContext.SaveChanges();
        }
        /// <summary>
        /// 执行事务
        /// </summary>
        /// <param name="action">需要执行的行为</param>
        /// <returns>返回给前端的数据类型</returns>
        public virtual WebResponseContent DbContextBeginTransaction(Func<WebResponseContent> action)
        {
            WebResponseContent webResponseContent = new WebResponseContent();
            using (IDbContextTransaction transaction = DbContext.Database.BeginTransaction())
            {
                try
                {
                    webResponseContent = action();
                    //获取执行结果
                    if (webResponseContent.Status)
                    {
                        transaction.Commit();
                    }
                    else
                    {
                        transaction.Rollback();
                    }
                    return webResponseContent;
                }
                catch (Exception ex)
                {
                   //如果执行失败 对事务进行回滚
                   transaction.Rollback();
                    return webResponseContent.Error(ex.ToString());
                }
            }
        }
    }
}
