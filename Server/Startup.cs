using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;
using ERacuni.Shared.Convertors;
using ERacuni.Shared.Models;
using ERacuni.Server.Context;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Grpc;
using ERacuni.Server.Services;
using MatBlazor;
using System;

namespace ERacuni.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<BillDBContext>(options =>
                options.UseSqlServer(
                        Configuration.GetConnectionString("ConnectionDbContext")).LogTo(Console.WriteLine,new[] { DbLoggerCategory.Database.Command.Name }, LogLevel.Information));

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddUserManager<UserManager<IdentityUser>>()
                .AddSignInManager<SignInManager<IdentityUser>>()
                .AddRoleManager<RoleManager<IdentityRole>>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<BillDBContext>();

            services.AddIdentityServer()
                .AddApiAuthorization<IdentityUser, BillDBContext>(options => {
                    options.IdentityResources["openid"].UserClaims.Add("name");
                    options.ApiResources.Single().UserClaims.Add("name");
                    options.IdentityResources["openid"].UserClaims.Add("role");
                    options.ApiResources.Single().UserClaims.Add("role");
                });

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("role");

            services.AddAuthentication().AddIdentityServerJwt();
            services.AddGrpc();
            services.AddMatBlazor();
            services.AddTransient<ConvertGRPC>();
          
            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();
            app.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled = true });
            app.UseRouting();
            app.UseIdentityServer();
            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapGrpcService<ERacuniService>();
                endpoints.MapGrpcService<BillService>();
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
