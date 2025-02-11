using FileDownLoadSystem.Core.Utilities.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileDownLoadSystem.System.Service
{
    public partial class FileTypeService
    {
        public async Task<WebResponseContent> GetFileTypeList()
        {
            return await Task.Run(() =>
            {
                var fileTypeList = _repository.FindAsIQueryable(m => m.Id != 0, m => new Dictionary<object, Core.Enums.QueryOrderBy>() { { m.Id, Core.Enums.QueryOrderBy.Asc } }).ToList();
                return WebResponseContent.Instance.OK(data: fileTypeList);
            });
        }
    }
}
