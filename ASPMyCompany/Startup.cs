using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

using ASPMyCompany.Service;
using ASPMyCompany.Domain.Repositories.Abstract;
using ASPMyCompany.Domain.Repositories.EntityFramework;
using ASPMyCompany.Domain;

namespace ASPMyCompany
{
    public class Startup
    {
        IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
            => Configuration = configuration;




        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // ���������� ������������ ������� �� appsettings.json � ������ Config
            Configuration.Bind("Project", new Config());

            // ���������� ������ ���������� ���������� � �������� ��������
            /* ����������� ������������ - � ������ ������ http-������� �� ����������
             * ����� ���� ������� ������� ������ �������� ������� ���� */
            services.AddTransient<ITextFieldsRepository, EFTextFieldsRepository>();
            services.AddTransient<IServiceItemsRepository, EFServiceItemsRepository>();
            services.AddTransient<DataManager>();

            // ���������� �� ����� ���� ������� �� appsettings.json
            services.AddDbContext<AppDbContext>(x => x.UseSqlServer(Config.ConnectionString));

            // ����������� identity-������� �������������
            services.AddIdentity<IdentityUser, IdentityRole>(option =>
            {
                option.User.RequireUniqueEmail = true;
                option.Password.RequiredLength = 6;
                option.Password.RequireNonAlphanumeric = false;
                option.Password.RequireLowercase = false;
                option.Password.RequireUppercase = false;
                option.Password.RequireDigit = false;
            }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

            // ����������� authentification cookie
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "MyCompanyAuth";
                options.Cookie.HttpOnly = true;
                options.LoginPath = "/account/login";
                options.AccessDeniedPath = "/account/accessdenied";
                options.SlidingExpiration = true;
            });

            // ��������� ��������� ������������
            services.AddControllersWithViews(x =>
                {
                    x.Conventions.Add(new AdminAreaAuthorisation("Admin", "AdminArea"));
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
        }




        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //���� ������ � ������ ����������, ���������� �������� ����������� ������
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            // ���������� ������� ������������� ( ����� ����� �������� ������� �� ��������� UseStaticFiles() ? )
            app.UseRouting();

            // ���������� ��������� ����������� ������
            app.UseStaticFiles();

            // ���������� �������������� � �����������
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();

            // ���������� ����������� ������� - ���������
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("admin", "{area:exist}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
