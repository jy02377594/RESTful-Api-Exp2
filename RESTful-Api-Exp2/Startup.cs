using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using RESTful_Api_Exp2.Data;
using RESTful_Api_Exp2.Services;
using System;

namespace RESTful_Api_Exp2
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddMvc();// 服务太多了不需要
            services.AddControllers(configure: setup =>
            {
                //请求类型和服务器返回类型不一致时返回406状态码
                setup.ReturnHttpNotAcceptable = true;
                //添加返回类型可以是xml
                setup.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
                //setup.OutputFormatters.Insert(index: 0, new XmlDataContractSerializerOutputFormatter());
            }
            );

            //添加对象映射器, Add AutoMapper
            //对象映射器能减少Controller里每次写entity对应的model
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            //.net core 3.0 以后关于增加返回类型新的写法
            /*services.AddControllers(configure: setup =>
            {
                setup.ReturnHttpNotAcceptable = true;
            }
            ).AddXmlDataContractSerializerFormatters();//.AddXmlSerializerFormatters();*/


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "RESTful_Api_Exp2", Version = "v1" });
            });
            //添加接口服务，接口和实现类
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IEmployeeTaskRepository, EmployeeTaskRepository>();

            //添加数据环境
            services.AddDbContext<Restful_DbContext>(optionsAction: option =>
            {
                option.UseSqlite(connectionString: "Data Source = RESTful_api.db");
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RESTful_Api_Exp2 v1"));
            }
            else { // 一般都是开发者模式，可以去properity -> debug -> Environment variables里设置，下面代码用来抛出服务器异常
                app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Run(handler: async context =>
                    {
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync(text: "Unexpected Error");
                    });
                });
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            //配置授权，放在ConfigureService里
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
