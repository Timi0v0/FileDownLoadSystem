using Autofac;
using FileDownLoadSystem.Core.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
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
            #region 自动注册服务
            // 拿到与本项目有关的项目
            var compilationLibrary = DependencyContext.Default.CompileLibraries
                .Where(x=>x.Type != "package" && x.Type == "project" && !x.Serviceable);
            // 获取所有的程序集
            List<Assembly> assemblies = new List<Assembly>();
            foreach (var assembly in compilationLibrary)
            { 
                assemblies.Add(AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(assembly.Name)));
            }
            // 找实现Idenpendency接口的类 并注册
            builder.RegisterAssemblyTypes(assemblies.ToArray()).
                Where(type=>!type.IsAbstract&&type.IsAssignableTo<IDenpendency>()).
                AsSelf().AsImplementedInterfaces().InstancePerLifetimeScope();
            #endregion

            return services;
        }
    }
}
