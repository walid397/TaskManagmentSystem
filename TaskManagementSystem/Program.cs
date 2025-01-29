using Domain.Models;
using Infrastructure.DbContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using AutoMapper;
using Infrastructure.Reposaitories;
using NuGet.Protocol.Core.Types;
using Application.Contract;
using Application.Services;
using Application.Mapper;


namespace TaskManagementSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<Context>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly("Infrastructure"));
            });
            builder.Services.AddIdentity<User, Role>
                (options=>
                {
                    options.Password.RequiredLength = 4;
                    options.Password.RequireDigit = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                }
                ).AddEntityFrameworkStores<Context>();
            builder.Services.AddAutoMapper(typeof(TaskAutoMapper));
            builder.Services.AddScoped<ITaskReposaitory, TaskReposaitory>();
            builder.Services.AddScoped<IServices, Services>(); 


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

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
