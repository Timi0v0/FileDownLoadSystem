using FileDownLoadSystem.Entity.AttributeManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileDownLoadSystem.Entity.FileInfo
{
    [Entity(DetailTable =new Type[] {typeof(FileMode)},ForeignKeyName = "FileTypeId")]
    public class FileTypes: BaseModel
    {
        public string? FileTypeName { get; set; }
    }
}
