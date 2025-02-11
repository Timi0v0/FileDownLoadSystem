using Autofac;
using Autofac.Extensions.DependencyInjection;
using FileDownLoadSystem.Core.Extensions;
using FileDownLoadSystem.System;
using Microsoft.AspNetCore.Builder;

namespace FileDownLoadSystem.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var configuration= builder.Configuration;
            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            builder.Host.ConfigureContainer<ContainerBuilder>(
                containerBuilder => { builder.Services.AddModule(containerBuilder, configuration); });
            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen();
            //���CORS�������ò���
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder=>builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });
            //���AutoMapper����
            builder.Services.AddAutoMapper(typeof(FileSystemProfile));

            var app = builder.Build();
            // ����swagger
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    // ���� Swagger UI ����ʼҳ��Ϊ��·�� "/"
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                    options.RoutePrefix = string.Empty; // ��·����ʾ Swagger UI
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.UseCors("AllowAllOrigins");

            app.MapControllers();

            app.Run();
        }
    }
}
