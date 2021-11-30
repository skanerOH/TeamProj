using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using PersonalBlog.BLL.Interfaces;
using PersonalBlog.BLL.Services;
using PersonalBlog.DAL;
using PersonalBlog.DAL.Entities;
using PersonalBlog.DAL.Interfaces;
using PersonalBlog.PL.Authentication;
using PersonalBlog.PL.Extentions;
using PersonalBlog.PL.Models.JWT;
using System.Text;

namespace PersonalBlog.PL
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
            services.Configure<JWTConfig>(Configuration.GetSection("JWTConfig"));

            services.AddDbContext<BlogsDBContext>(opt =>
            {
                opt.UseSqlServer(Configuration.GetConnectionString("ProjDB"));
            });

            services.AddIdentity<UserWithIdentity, IdentityRole>(opt => { })
                .AddEntityFrameworkStores<BlogsDBContext>();

            services.AddAutoMapper(typeof(AutomapperProfile));

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IUserManagementService, UserManagementService>();
            services.AddScoped<IBlogService, BlogService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IArticleService, ArticleService>();
            services.AddScoped<ITagService, TagService>();

            services.AddScoped<IAppUser, AppUser>();

            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(opt =>
                {
                    var key = Encoding.ASCII.GetBytes(Configuration["JWTConfig:Key"]);
                    var issuer = Configuration["JWTConfig:Issuer"];
                    var audience = Configuration["JWTConfig:Audience"];
                    opt.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        RequireExpirationTime = false,
                        ValidIssuer = issuer,
                        ValidAudience = audience
                    };
                });

            services.AddCors(opt =>
            {
                opt.AddPolicy(Configuration["Cors:PolicyName"], builder =>
                {
                    builder.AllowAnyOrigin();
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                });
            });

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddControllersWithViews();
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            //else
            //{
            //    app.UseExceptionHandler("/Error");
            //    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            //    app.UseHsts();
            //}

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();

            app.UseCors(Configuration["Cors:PolicyName"]);

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
