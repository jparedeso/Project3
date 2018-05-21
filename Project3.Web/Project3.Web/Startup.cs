using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Project3.Web.Data;
using Project3.Web.Models;
using Project3.Web.Services;

namespace Project3.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, CustomRole>(option =>
                {
                    option.Password.RequireDigit = true;
                    option.Password.RequiredLength = 6;
                    option.Password.RequiredUniqueChars = 0;
                    option.Password.RequireLowercase = true;
                    option.Password.RequireNonAlphanumeric = true;
                    option.Password.RequireUppercase = true;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddMvc().AddSessionStateTempDataProvider();

            services.AddDistributedMemoryCache(); // Adds a default in-memory implementation of IDistributedCache
            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.Cookie = new CookieBuilder
                {
                    Domain = "/",
                    Expiration = TimeSpan.MaxValue,
                    HttpOnly = true,
                    Name = "AspNetSession"
                };
            });

            services.AddSingleton(Configuration.GetSection("AppSettings").Get<AppSettings>());
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            return services.BuildServiceProvider();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

//            if (env.IsDevelopment() || env.IsStaging())
//            {
//                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
//                {
//                    HotModuleReplacement = true,
//                    ReactHotModuleReplacement = true
//                });
//            }

            //using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            //{
            //    var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            //    context.Database.Migrate();
            //}

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseGetRoutesMiddleware(GetRoutes);

            app.UseSession();

            app.UseMvc(GetRoutes);
        }

        private readonly Action<IRouteBuilder> GetRoutes =
            routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            };
    }
}
