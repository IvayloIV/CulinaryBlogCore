using CulinaryBlogCore.DataAccess;
using CulinaryBlogCore.Data.Models.Identity;
using CulinaryBlogCore.Services.Contracts;
using CulinaryBlogCore.Services;
using CulinaryBlogCore.Services.Repository.Contracts;
using CulinaryBlogCore.Services.Repository;
using CulinaryBlogCore.Utils;
using CulinaryBlogCore.Services.Services;

using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CulinaryBlogCore.Services.Utils;

namespace CulinaryBlogCore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddDbContext<CulinaryBlogDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<CulinaryBlogDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 3;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            });

            // Add application services.
            services.AddScoped<IRepository, Repository>();
            services.AddScoped<IRecipeService, RecipeService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IMailService, MailService>();
            services.AddScoped<IImgurTokenService, ImgurTokenService>();
            services.AddScoped<IChefService, ChefService>();

            services.Configure<SubscriptionMailData>(Configuration.GetSection("SubscriptionMailData"));
            services.Configure<ImgurConfigData>(Configuration.GetSection("ImgurConfigData"));

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapperConfig());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // Seed data on application startup

            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            //db.Database.Migrate();

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }
    }
}
