using FileDownLoadSystem.Core.Enums;
using FileDownLoadSystem.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileDownLoadSystem.Core.Utilities.Response
{
    /// <summary>
    /// 返回前端实体的封装
    /// </summary>
    public class WebResponseContent
    {
        public bool Status { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        //public string Message { get; set; }
        public object Data { get; set; }
        /// <summary>
        /// 设计符合链式法则 例如:WebResponseContent.Instance.OK("成功")
        /// </summary>
        public static WebResponseContent Instance=>new WebResponseContent();
        public WebResponseContent()
        {
                
        }

        public WebResponseContent OK()
        {
            this.Status = true;
            return this;
        }

        public WebResponseContent OK(string message=null,object data = null)
        {
            this.Status = true;
            this.Message = message;
            this.Data = data;
            return this;
        }
        public WebResponseContent Error()
        {
            this.Status = false;
            return this;
        }
        public WebResponseContent Error(string message = null, object data = null)
        {
            this.Status = false;
            this.Message = message;
            this.Data = data;
            return this;
        }

        public WebResponseContent Set(ResponseType responseType, string message = null, object data = null)
        {
            this.Code = ((int)responseType).ToString();
            if (data!=null)
            {
                this.Data = data;
            }
            if (!string.IsNullOrEmpty(message))
            {
                this.Message = message;
            }
            else
            {
                this.Message = responseType.GetMsg();
            }
            return this;
        }
    }
}
