namespace BooksForYou.Web
{
    using System;
    using System.Configuration;
    using System.Globalization;
    using System.Reflection;

    using BooksForYou.Data;
    using BooksForYou.Data.Common;
    using BooksForYou.Data.Common.Repositories;
    using BooksForYou.Data.Models;
    using BooksForYou.Data.Repositories;
    using BooksForYou.Data.Seeding;
    using BooksForYou.Services.AzureServices;
    using BooksForYou.Services.Data.Books;
    using BooksForYou.Services.Data.Genres;
    using BooksForYou.Services.Data.Languages;
    using BooksForYou.Services.Data.Publishers;
    using BooksForYou.Services.Data.Users;
    using BooksForYou.Services.Data.Votes;
    using BooksForYou.Services.GoogleReCaptcha;
    using BooksForYou.Services.Mapping;
    using BooksForYou.Services.Messaging;
    using BooksForYou.Web.ViewModels;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            ConfigureServices(builder.Services, builder.Configuration);
            var app = builder.Build();
            Configure(app);

            app.Run();
        }

        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<ApplicationUser>(IdentityOptionsProvider.GetIdentityOptions)
                .AddRoles<ApplicationRole>().AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddAuthentication()
                .AddFacebook(options =>
                {
                    options.AppId = configuration.GetValue<string>("Facebook:AppId");
                    options.AppSecret = configuration.GetValue<string>("Facebook:AppSecret");
                });

            services.Configure<GoogleReCaptchaConfig>(configuration.GetSection("GoogleReCaptcha"));
            services.Configure<AzureOptConfig>(configuration.GetSection("Azure"));

            services.Configure<CookiePolicyOptions>(
                options =>
                {
                    options.CheckConsentNeeded = context => true;
                    options.MinimumSameSitePolicy = SameSiteMode.None;
                });

            services.AddControllersWithViews()
                .AddMvcOptions(options =>
                {
                    options.Filters.Add<AutoValidateAntiforgeryTokenAttribute>();
                }).AddRazorRuntimeCompilation();
            services.AddRazorPages();
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddAntiforgery(options =>
            {
                options.HeaderName = "X-CSRF-TOKEN";
            });

            services.AddSingleton(configuration);

            // Data repositories
            services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<IDbQueryRunner, DbQueryRunner>();

            // Application services
            services.AddTransient<IEmailSender>(x => new SendGridEmailSender(configuration.GetValue<string>("SendGridApiKey")));
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<IGenresService, GenresService>();
            services.AddScoped<IBooksService, BooksService>();
            services.AddScoped<ILanguagesService, LanguagesService>();
            services.AddScoped<IPublishersService, PublishersService>();
            services.AddSingleton<IAzureImageService, AzureImageService>();
            services.AddTransient(typeof(GoogleReCaptchaService));
            services.AddScoped<IVotesService, VotesService>();
        }

        private static void Configure(WebApplication app)
        {
            // Seed data on application startup
            using (var serviceScope = app.Services.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.Migrate();
                new ApplicationDbContextSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();
            }

            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute("areaRoute", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
            app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();
        }
    }
}
