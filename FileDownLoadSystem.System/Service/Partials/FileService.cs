using FileDownLoadSystem.Core.BaseProvider;
using FileDownLoadSystem.Core.Enums;
using FileDownLoadSystem.Core.Utilities.Response;
using FileDownLoadSystem.Entity.FileInfo;
using FileDownLoadSystem.System.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileDownLoadSystem.System.Service
{
    public partial class FileService
    {
        public async Task<WebResponseContent> GetFilesByType(int typeId)
        {
            return await Task.Run(() =>
            {
                var files = _repository.FindAsIQueryable(m => m.FileTypeId == typeId)
                                       .Include(m => m.FilePackages)
                                       .OrderByDescending(m => m.DownloadCount)
                                       
                                       .ToList();
                // var filedtos = _mapper.Map<List<FileModel>, List<FileDto>>(files);
                return WebResponseContent.Instance.OK(data: files);
            });
        }
    }
}
