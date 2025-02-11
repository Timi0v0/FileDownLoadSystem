using AutoMapper;
using FileDownLoadSystem.Entity.FileInfo;
using FileDownLoadSystem.System.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileDownLoadSystem.System
{
    public class FileSystemProfile:Profile
    {
        public FileSystemProfile() {
            // 定义 FileModel 和 FileDto 之间的映射
            CreateMap<FileModel,FileDto>();
            CreateMap<FileDto, FileModel>();
        }
    }
}
