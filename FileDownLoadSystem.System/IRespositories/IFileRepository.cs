using FileDownLoadSystem.Core.BaseProvider;
using FileDownLoadSystem.Core.Extensions;
using FileDownLoadSystem.Entity.FileInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileDownLoadSystem.System.IRespositories
{
    public interface IFileRepository : IRepository<FileModel>, IDenpendency
    {
    }
}
