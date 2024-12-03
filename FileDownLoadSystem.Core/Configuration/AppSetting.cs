using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileDownLoadSystem.Core.Configuration
{
    /// <summary>
    /// 对应appsettings.json配置文件
    /// </summary>
    public static class AppSetting
    {
        private static Connection _connection;
        public static void Init(IServiceCollection services, IConfiguration configuration)
        {
            //从appsettings.json加载对应节点的文件
            _connection = configuration.GetSection("Connection").Get<Connection>();
        }


    }
    public class Connection
    {
        public string DBType { get; set; }
        public string DbConnectionString { get; set; }
        public string RedisConnectionString { get; set; }
        public bool UseRedis { get; set; }
        public bool UseSignalR { get; set; }
    }
}
