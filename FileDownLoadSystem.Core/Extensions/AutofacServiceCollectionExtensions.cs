using Autofac;
using FileDownLoadSystem.Core.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileDownLoadSystem.Core.Extensions
{
    public static class AutofacServiceCollectionExtensions
    {
        /// <summary>
        /// 将注册服务通过扩展方法的形式来实现将注册步骤转移到Core层
        /// </summary>
        /// <param name="services">扩展方法的使用类</param>
        /// <param name="builder">需要承载服务的ioc容器</param>
        /// <param name="configuration">配置</param>
        /// <returns></returns>
        public static IServiceCollection AddModule(this IServiceCollection services,ContainerBuilder builder,IConfiguration configuration) {
            // 初始化配置文件
            AppSetting.Init(services, configuration);
            return services;
        }
    }
}
