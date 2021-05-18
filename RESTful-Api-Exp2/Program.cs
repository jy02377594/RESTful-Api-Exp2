using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RESTful_Api_Exp2.Data;
using Microsoft.EntityFrameworkCore;

namespace RESTful_Api_Exp2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            
            using (var scope = host.Services.CreateScope())
            {
                try {
                    var dbContext = scope.ServiceProvider.GetService<Restful_DbContext>();
                    dbContext.Database.EnsureDeleted();
                    //��ȥconsloeִ��Add-Migration initialMigration,����ɾ��֮ǰmigrations�ļ���������ļ������ܸ��������ݣ���ִ�����ִ�к��ı����ݿ�����
                    dbContext.Database.Migrate();
                }
                catch(Exception e) {
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                    logger.LogError(e, message: "Database Migration Error!");
                }

            }
                host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
