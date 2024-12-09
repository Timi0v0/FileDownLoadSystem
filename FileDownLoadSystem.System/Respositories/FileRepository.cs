using FileDownLoadSystem.Core.BaseProvider;
using FileDownLoadSystem.Core.EFDbContext;
using FileDownLoadSystem.Entity.FileInfo;
using FileDownLoadSystem.System.IRespositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FileDownLoadSystem.System.Respositories
{
    public class FileRepository : RepositoryBase<FileModel>, IFileRepository
    {
        public FileRepository(FileDownLoadSystemDbContext dbContext) : base(dbContext)
        {
        }

        public IQueryable<FileModel> FindAsIQueryable(Expression<Func<FileModel, bool>> predicate)
        {
            return base.FindAsIQueryable(predicate);
        }

        public FileModel FindFirst(Expression<Func<FileModel, bool>> predicate)
        {
            return base.FindFirst(predicate);
        }
    }
}
