using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileDownLoadSystem.Entity.FileInfo
{
    public class FilePackages:BaseModel 
    {
        public int FileId { get; set; }

        public string PackageUrl { get; set; }

        public string PackageName { get; set; }

        public DateTime? PublishTime { get; set; }
    }
}
