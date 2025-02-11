using FileDownLoadSystem.Core.BaseProvider;
using FileDownLoadSystem.Core.Extensions;
using FileDownLoadSystem.Core.Utilities.Response;
using FileDownLoadSystem.Entity.FileInfo;
using FileDownLoadSystem.System.IRespositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileDownLoadSystem.System.IService
{
    public interface IFileWebConfigsService : IService<FileWebConfigs, IFileWebConfigsRespository>, IDenpendency
    {
        public Task<WebResponseContent> GetFileWebConfigs();
    }
}
