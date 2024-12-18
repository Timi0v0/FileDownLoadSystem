using FileDownLoadSystem.Core.EFDbContext;
using FileDownLoadSystem.Core.Enums;
using FileDownLoadSystem.Core.Extensions;
using FileDownLoadSystem.Core.Utilities.Response;
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
        public FileDownLoadSystemDbContext DbContext {  get; }
        public TBaseModel FindFirst(Expression<Func<TBaseModel, bool>> predicate);

        public IQueryable<TBaseModel> FindAsIQueryable(Expression<Func<TBaseModel, bool>> predicate);

        public TBaseModel FindFirst(Expression<Func<TBaseModel, bool>> predicate, Expression<Func<TBaseModel, Dictionary<object, QueryOrderBy>>> orderBy = null);

        public IQueryable<TBaseModel> FindAsIQueryable(Expression<Func<TBaseModel, bool>> predicate, Expression<Func<TBaseModel, Dictionary<object, QueryOrderBy>>> orderBy = null);

        public  void AddRange(IEnumerable<TBaseModel> baseModels, bool isSave = false);

        /// <summary>
        /// 将某个数据添加到数据库中
        /// </summary>
        /// <param name="baseModel">实体对象</param>
        /// <param name="isSave">是否提交到数据库</param>
        public void Add(TBaseModel baseModel, bool isSave = false);
        /// <summary>
        /// 异步将某些数据添加到数据库中
        /// </summary>
        /// <param name="baseModel"></param>
        /// <returns></returns>
        public Task AddRangeAsync(IEnumerable<TBaseModel> baseModels);
        /// <summary>
        /// 异步将某个数据添加到数据库中
        /// </summary>
        /// <param name="baseModel"></param>
        /// <returns></returns>
        public Task AddAsync(TBaseModel baseModel);
        /// <summary>
        /// 删除某个数据
        /// </summary>
        /// <param name="baseModel"></param>
        /// <param name="isSave"></param>
        public void Delete(TBaseModel baseModel, bool isSave = false);
        /// <summary>
        /// 批量删除某些数据
        /// </summary>
        /// <param name="baseModels"></param>
        /// <param name="isSave"></param>
        public void DeleteRange(IEnumerable<TBaseModel> baseModels, bool isSave = false);
        /// <summary>
        /// 根据条件删除某些数据 通过此方法进行删除性能比较低 执行原生态的SQL语句效率会更高
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="isSave"></param>
        public void DeleteRange(Expression<Func<TBaseModel, bool>> predicate, bool isSave = false);
        /// <summary>
        /// 通过主键批量删除
        /// </summary>
        /// <param name="keys">主键key</param>
        /// <param name="delList">是否连明细一起删除</param>
        public void DeleteWithKeys(object[] keys, bool delList = false);

        /// <summary>
        /// 更新数据库
        /// </summary>
        /// <param name="baseModel"></param>

        public int Update(TBaseModel baseModel);
        /// <summary>
        /// 开放其他实体类去使用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="baseModel"></param>
        /// <returns></returns>
        public int Update<T>(T baseModel) where T : class;
        /// <summary>
        /// 保存更改
        /// </summary>
        public  void SaveChanges();

        /// <summary>
        /// 执行事务
        /// </summary>
        /// <param name="action">需要执行的行为</param>
        /// <returns>返回给前端的数据类型</returns>
        public  WebResponseContent DbContextBeginTransaction(Func<WebResponseContent> action);


    }
}
