using FileDownLoadSystem.Core.Enums;
using FileDownLoadSystem.Core.Utilities.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FileDownLoadSystem.System.Service
{
    /// <summary>
    /// 实现具体的业务逻辑
    /// </summary>
    public partial class FileWebConfigsService
    {
        public async Task<WebResponseContent> GetFileWebConfigs()
        {
            return await Task.Run(() =>
            {
                var data = _repository.FindAsIQueryable(m => m.Id != 0, m => new Dictionary<object, Core.Enums.QueryOrderBy>() { { m.PublishTime, QueryOrderBy.Desc } }).FirstOrDefault();
                return WebResponseContent.Instance.OK(data: data);
            } );
        }
    }
}
