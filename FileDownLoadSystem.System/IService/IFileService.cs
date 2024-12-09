using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileDownLoadSystem.Core.BaseProvider;
using FileDownLoadSystem.Core.Extensions;
using FileDownLoadSystem.Entity.FileInfo;
using FileDownLoadSystem.System.IRespositories;
namespace FileDownLoadSystem.System.IService
{
    public interface IFileService:IService<FileModel, IFileRepository>,IDenpendency
    {
    }
}
