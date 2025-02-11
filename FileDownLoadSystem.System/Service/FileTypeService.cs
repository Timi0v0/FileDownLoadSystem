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
    public partial class FileTypeService : BaseService<FileTypes, IFileTypesRespository>, IFileTypeService
    {
        public FileTypeService(IFileTypesRespository baseRepository) : base(baseRepository)
        {
        }
    }
}
