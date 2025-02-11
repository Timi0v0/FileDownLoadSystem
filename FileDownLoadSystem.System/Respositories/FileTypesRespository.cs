using FileDownLoadSystem.Core.BaseProvider;
using FileDownLoadSystem.Core.EFDbContext;
using FileDownLoadSystem.Entity.FileInfo;
using FileDownLoadSystem.System.IRespositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileDownLoadSystem.System.Respositories
{
    public class FileTypesRespository : RepositoryBase<FileTypes>, IFileTypesRespository
    {
        public FileTypesRespository(FileDownLoadSystemDbContext dbContext) : base(dbContext)
        {
        }
    }
}
