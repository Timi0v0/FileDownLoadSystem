using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileDownLoadSystem.System.Dto
{
    public class FileDto
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public DateTime UploadTime
        {
            get; set;
        }
    }
}
