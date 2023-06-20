using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using DatabaseConection.Entities;
using DataService.AccountService;
using DataService.AdviseServices;
using DataService.AuthServices;
using DataService.CategoryMappingServices;
using DataService.CategoryServices;
using DataService.ChatServices;
using DataService.EmployeeServices;
using DataService.ExpertServices;
using DataService.HashService;
using DataService.RatingServices;
using DataService.UserServices;
using Microsoft.Extensions.Hosting;
using static DataService.RatingServices.IRatingService;

namespace ExpertConnect
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
           
            builder.Services.AddControllers();
            builder.Services.AddSwaggerGen();
          
            // Dependency
            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
            { 
                builder.RegisterType<HashService>().As<IHashService>();
                builder.RegisterType<ExpertConectionContext>().AsSelf();
                builder.RegisterType<AccountService>().As<IAccountService>();
                builder.RegisterType<EmployeeService>().As<IEmployeeService>();
                builder.RegisterType<UserService>().As<IUserService>();
                builder.RegisterType<AuthService>().As<IAuthService>();
                builder.RegisterType<CategoryService>().As<ICategoryService>();
                builder.RegisterType<ExpertService>().As<IExpertService>(); 
                builder.RegisterType<CategoryMappingService>().As<ICategoryMappingService>();
                builder.RegisterType<AdviseService>().As<IAdviseService>();
                builder.RegisterType<ChatService>().As<IChatService>();
                builder.RegisterType<RatingService>().As<IRatingService>();
            });

            //config swagger
            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                    options.RoutePrefix = string.Empty;
                });
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}