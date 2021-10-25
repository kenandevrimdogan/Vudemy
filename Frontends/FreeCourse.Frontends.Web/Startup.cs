using FluentValidation.AspNetCore;
using FreeCourse.Frontends.Web.Extensions;
using FreeCourse.Frontends.Web.Handler;
using FreeCourse.Frontends.Web.Helpers;
using FreeCourse.Frontends.Web.Models;
using FreeCourse.Frontends.Web.Services.Abstracts;
using FreeCourse.Frontends.Web.Services.Interfaces;
using FreeCourse.Frontends.Web.Validators.Courses;
using FreeCourse.Shared.Services.Abstracts;
using FreeCourse.Shared.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace FreeCourse.Frontends.Web
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
            var serviceApiSettings = Configuration.GetSection("ServiceApiSettings").Get<ServiceApiSettings>();
            services.AddHttpContextAccessor();
            services.AddAccessTokenManagement();

            services.AddSingleton<PhotoHelper>();

            services.AddScoped<ISharedIdentityService, SharedIdentityService>();
            services.AddScoped<ResourceOwnerPasswordTokenHandler>();
            services.AddScoped<ClientCredentialTokenHandler>();

            services.AddHttpClientServices(Configuration);
            services.AddHttpClient<IUserService, UserService>(opt =>
            {
                opt.BaseAddress = new Uri(serviceApiSettings.IdentityBaseUri);
            }).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();


            services.Configure<ServiceApiSettings>(Configuration.GetSection("ServiceApiSettings"));
            services.Configure<ClientSettings>(Configuration.GetSection("ClientSettings"));

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, opts =>
            {
                opts.LoginPath = "/Auth/SignIn";
                opts.ExpireTimeSpan = TimeSpan.FromDays(60);
                opts.SlidingExpiration = true;
                opts.Cookie.Name = "vudemywebcookie";
            });
            services.AddControllersWithViews().AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CourseCreateInputValidator>());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
