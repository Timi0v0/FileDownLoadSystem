using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileDownLoadSystem.Entity.FileInfo
{
    public class FileWebConfigs: BaseModel
    {
        public string Content { get; set; }

        public DateTime PublishTime { get; set; }
    }
}
