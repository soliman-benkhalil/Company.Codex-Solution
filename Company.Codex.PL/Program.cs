using Company.Codex.BLL;
using Company.Codex.BLL.Interfaces;
using Company.Codex.BLL.Repositories;
using Company.Codex.DAL.Data.Contexts;
using Company.Codex.DAL.Models;
using Company.Codex.PL.Mapping;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Data;
using System.Reflection;

namespace Company.Codex.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            //builder.Services.AddScoped<AppDbContext>(); // Allow DI For AppDbContext

            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")); 
            });

            builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>(); // Allow DI For DepartmentRepository

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()  // Allow DI For ApplicationUser And For The Store Which Represents The Repository In UserManager
                .AddDefaultTokenProviders();


			builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();



            builder.Services.AddAutoMapper(typeof(EmployeeProfile));


            builder.Services.AddAutoMapper(typeof(DepartmentProfile)); 


            builder.Services.ConfigureApplicationCookie(C =>
            {
                C.LoginPath = "/Account/SignIn";
			});



            var app = builder.Build();

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

            // 2 layer to limit the access to the pages according to a specific user
            app.UseAuthentication(); 
            app.UseAuthorization();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
