using LeapPlannerApi.Repository;
using LeapPlannerApi.Repository.Login;
using LeapPlannerApi.Service.Login;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AutoMapper;
using LeapPlannerApi.Mapper;
using LeapPlannerApi.Service.Planner;
using LeapPlannerApi.Repository.Planner;
using LeapPlannerApi.Service.Common;
using LeapPlannerApi.Service.TaskCard;
using LeapPlannerApi.Repository.TaskCard;
using LeapPlannerApi.Service.Task;
using LeapPlannerApi.Repository.Task;

namespace LeapPlannerApi
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
            services.AddSingleton<DapperContext>();
            services.AddControllers();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                policyBuilder => policyBuilder.AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin()
                    //.AllowCredentials()
                    .SetIsOriginAllowed(_ => true)
                 );
            });

            #region Configure services
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<IPlannerService, PlannerService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ITaskCardService, TaskCardService>();
            services.AddScoped<ITaskService, TaskService>();
            #endregion  Configure services

            #region Configure repo
            services.AddScoped<ILoginRepo, LoginRepo>();
            services.AddScoped<IPlannerRepo, PlannerRepo>();
            services.AddScoped<ITaskCardRepo, TaskCardRepo>();
            services.AddScoped<ITaskRepo, TaskRepo>();
            #endregion configure repo

            #region Add mapper
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new LoginMapper());
                mc.AddProfile(new PlannerMapper());
                mc.AddProfile(new TaskCardMapper());
                mc.AddProfile(new TaskMapper());
            });
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
            #endregion Add mapper
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseHttpsRedirection();
            app.UseCors("AllowAllOrigins");
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
