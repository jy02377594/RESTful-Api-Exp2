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
            //services.AddMvc();// ����̫���˲���Ҫ
            services.AddControllers(configure: setup =>
            {
                //�������ͺͷ������������Ͳ�һ��ʱ����406״̬��
                setup.ReturnHttpNotAcceptable = true;
                //��ӷ������Ϳ�����xml
                setup.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
                //setup.OutputFormatters.Insert(index: 0, new XmlDataContractSerializerOutputFormatter());
            }
            );

            //��Ӷ���ӳ����, Add AutoMapper
            //����ӳ�����ܼ���Controller��ÿ��дentity��Ӧ��model
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            //.net core 3.0 �Ժ�������ӷ��������µ�д��
            /*services.AddControllers(configure: setup =>
            {
                setup.ReturnHttpNotAcceptable = true;
            }
            ).AddXmlDataContractSerializerFormatters();//.AddXmlSerializerFormatters();*/


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "RESTful_Api_Exp2", Version = "v1" });
            });
            //��ӽӿڷ��񣬽ӿں�ʵ����
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IEmployeeTaskRepository, EmployeeTaskRepository>();

            //������ݻ���
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
            else { // һ�㶼�ǿ�����ģʽ������ȥproperity -> debug -> Environment variables�����ã�������������׳��������쳣
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

            //������Ȩ������ConfigureService��
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
