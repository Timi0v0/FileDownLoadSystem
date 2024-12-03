using FileDownLoadSystem.Core.Extensions;
using FileDownLoadSystem.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;

namespace FileDownLoadSystem.Core.EFDbContext
{
    public class FileDownLoadSystemDbContext: DbContext,IDenpendency
    {
        public FileDownLoadSystemDbContext()
        {               
        }
        public FileDownLoadSystemDbContext(DbContextOptions<FileDownLoadSystemDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                optionsBuilder.UseMySql("Database=FileDownLoadSystemDb;Data Source=localhost;Port=3306;User Id=root;Password=tm011028", MySqlServerVersion.LatestSupportedServerVersion);
            }
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            try
            {
                //通过反射创建所有的实体类 避免在类中进行声明
                //获取所有的类库
                var compilationLibrary = DependencyContext.Default.CompileLibraries.
                    Where(x => x.Type != "package" && x.Type == "project" && !x.Serviceable);
                //获取所有的数据模型 
                foreach (var item in compilationLibrary)
                {
                    AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(item.Name)).
                        GetTypes().
                        Where(x => x.GetTypeInfo().BaseType != null && !x.IsAbstract && x.BaseType == typeof(BaseModel)).
                        ToList().ForEach(x =>
                        {
                            modelBuilder.Entity(x);
                        });
                }
            }
            catch (Exception)
            {

                throw;
            }
            base.OnModelCreating(modelBuilder);
        }
    }
}
