using EmployeeManagement.Models;
using EmployeeManagement.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement
{
    public class Startup
    {
        private IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>(x =>
            {
                //overriding password complexity
                x.Password.RequiredLength = 8;
                x.Password.RequiredUniqueChars = 3;

            }).AddEntityFrameworkStores<AppDbContext>();

            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = new PathString("/Administration/AccessDenied");
            });


            services.AddMvc(options => {
                var policy = new AuthorizationPolicyBuilder().
                RequireAuthenticatedUser().Build();
                options.Filters.Add(new AuthorizeFilter(policy));
                options.EnableEndpointRouting = false;
            }).AddXmlSerializerFormatters();

            //services.AddAuthentication().AddGoogle



           services.AddAuthorization(options =>
           {
               options.AddPolicy("EditRolePolicy", policy =>
                   policy.AddRequirements(new ManageAdminRolesAndClaimsRequirement()));
               options.InvokeHandlersAfterFailure = false;
           });

            // Register the first handler
            services.AddSingleton<IAuthorizationHandler,
                CanEditOnlyOtherAdminRolesAndClaimsHandler>();
            // Register the second handler
            services.AddSingleton<IAuthorizationHandler, SuperAdminHandler>();

            services.AddDbContextPool<AppDbContext>(options => options.UseSqlServer(_config.GetConnectionString("EmployeeDBConnection")));


            services.AddScoped<IEmployeeRepository, SQLEmployeeRepositry>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                //DeveloperExceptionPageOptions developerExceptionPageOptions = new DeveloperExceptionPageOptions()
                //{
                //    SourceCodeLineCount = 2
                //};
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseStatusCodePagesWithReExecute("/error/{0}");
            }
           
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvc(route =>
            {
                route.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
