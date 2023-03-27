using FoodDeliveryWebApp.Areas.Identity.Data;
using FoodDeliveryWebApp.Contracts;
using FoodDeliveryWebApp.Data;
using FoodDeliveryWebApp.Models.Categories;
using FoodDeliveryWebApp.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace FoodDeliveryWebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder
                .Configuration.GetConnectionString("FoodDeliveryWebAppContextConnection") ?? throw new InvalidOperationException("Connection string 'FoodDeliveryWebAppContextConnection' not found.");

            #region Services
            builder.Services
                .AddDbContext<FoodDeliveryWebAppContext>(
                options => options.UseSqlServer(connectionString));

            //builder.Services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<FoodDeliveryWebAppContext>();

            //builder.Services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<FoodDeliveryWebAppContext>();

            #region Authentication Services
            //builder.Services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<FoodDeliveryWebAppContext>();

            builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<FoodDeliveryWebAppContext>().AddDefaultTokenProviders();

            builder.Services.Configure<DataProtectionTokenProviderOptions>(opts => opts.TokenLifespan = TimeSpan.FromHours(10));

            builder.Services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;
            });

            builder.Services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);

                options.LoginPath = "/Identity/Account/Login";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.SlidingExpiration = true;
            });

            builder.Services.AddAuthentication();

            builder.Services.AddAuthorization();
            #endregion

            #region Repository Services
            builder.Services.AddScoped<ICustomerHomeRepo, CustomerHomeRepo>();
            builder.Services.AddScoped<ISellerRepo, SellerRepo>();
            builder.Services.AddScoped<IModelRepo<Category>, CategoryRepo>();
            builder.Services.AddScoped<ModelRepo<FoodDeliveryWebApp.Models.Product>, ProductRepo>();
            #endregion

            builder.Services.AddRazorPages();

            builder.Services.AddControllersWithViews();
            #endregion

            var app = builder.Build();
            StripeConfiguration.ApiKey = "sk_test_51Mq0DEDRs2d2XncX3l5gLODG0on2gtdtEiPEXSsyB2m2TUfGwZwlanLbn5ZBZGP3LJbOjDXlsx1f5j0eTcKbKKJI00mPVX4uAc";

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            {
                // DON't CHANGE THEIR ORDER
                app.UseAuthentication();
                app.UseAuthorization();
            }

            app.MapControllerRoute(
                name: "defaultWithArea",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
            );

            app.MapRazorPages();

            //app.MapControllerRoute(
            //    name: "defaultWithArea",
            //    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
            //);

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}