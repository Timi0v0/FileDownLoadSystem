﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileDownLoadSystem.Entity.FileInfo
{
    /// <summary>
    /// 下载文件模型
    /// </summary>
    public class FileModel:BaseModel
    {
        //文件序号
        public int FileId { get; set; }
        //文件名称
        public string FileName { get; set; }
        //文件图标地址
        public string? FileIconUrl { get; set; }
        //文件的上传时间
        public DateTime UploadTime { get; set; }

        //文件的点击次数
        public long ClickCount { get; set; }
        //文件的下载次数
        public long DownloadCount { get; set; }
        //文件的描述
        public string? FileDescription { get; set; }
    }
}
