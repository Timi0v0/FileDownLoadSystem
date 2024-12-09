using FileDownLoadSystem.Core.BaseProvider;
using FileDownLoadSystem.Entity.FileInfo;
using FileDownLoadSystem.System.IRespositories;
using FileDownLoadSystem.System.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileDownLoadSystem.System.Service
{
    public class FileService : BaseService<FileModel, IFileRepository>, IFileService
    {
        public FileService(IFileRepository baseRepository) : base(baseRepository)
        {
        }
    }
}
