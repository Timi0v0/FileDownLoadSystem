using FileDownLoadSystem.Core.BaseProvider;
using FileDownLoadSystem.Core.EFDbContext;
using FileDownLoadSystem.Entity.FileInfo;
using FileDownLoadSystem.System.IRespositories;
using FileDownLoadSystem.System.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FileDownLoadSystem.System.Respositories
{
    public class FileWebConfigsRespository : RepositoryBase<FileWebConfigs>,IFileWebConfigsRespository
    {
        public FileWebConfigsRespository(FileDownLoadSystemDbContext dbContext) : base(dbContext)
        {
        }
        public override FileWebConfigs FindFirst(Expression<Func<FileWebConfigs, bool>> predicate)
        {
            return base.FindFirst(predicate);
        }
    }
}
