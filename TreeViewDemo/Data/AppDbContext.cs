﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TreeViewDemo.Models;

namespace TreeViewDemo.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor httpContextAccessor) : DbContext(options)
    {
        public readonly IHttpContextAccessor HttpContextAccessor = httpContextAccessor;
        private AppUser LoggedInUser { get; set; }
        public DbSet<Person> Persons { get; init; }
        public DbSet<AppUser> AppUsers { get; init; }
        public DbSet<AppUserLoginHistory> AppUserLoginHistories { get; init; }

        public AppUser GetLoggedInUser
        {
            get
            {
                if (LoggedInUser != null) return LoggedInUser;
                var cookie = HttpContextAccessor?.HttpContext?.Request.Cookies[Globals.AuthenticationCookieName];
                LoggedInUser = AppUserLoginHistories.Where(m => m.Token == cookie).Select(m => m.User).FirstOrDefault();

                return LoggedInUser;
            }
        }

        public int GetLoggedInUserId => GetLoggedInUser?.Id ?? 0;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<AppUser>().HasData(
            //    new AppUser { Id = 1, LoginId = "admin", Password = "Hn41zmfdUEbuEwHaAGTwmw==", TreeName = "IT Tree" },
            //    new AppUser { Id = 2, LoginId = "admin1", Password = "V2tKjVyHS//VCwcCSf4ywQ==", TreeName = null }
            //);

            //modelBuilder.Entity<Person>().HasData(
            //    new Person { Id = 6, Name = "Software Development", Status = true, TextColor = "#000000", BgColor = "#ffffff", UserId = 1, PhotoUrl = "/persons/oeg4mbcrgqj.png" },
            //    new Person { Id = 7, Name = "App Development", Status = true, ParentId = 6, TextColor = "#000000", BgColor = "#ffffff", UserId = 1 },
            //    new Person { Id = 8, Name = "Android Apps", Status = true, ParentId = 7, TextColor = "#000000", BgColor = "#ffffff", UserId = 1 },
            //    new Person { Id = 9, Name = "Web Development", Status = true, ParentId = 6, TextColor = "#000000", BgColor = "#ffffff", UserId = 1, PhotoUrl = "/persons/otev5i133bt.jpeg" },
            //    new Person { Id = 10, Name = "Desktop Development", Status = true, ParentId = 6, TextColor = "#000000", BgColor = "#ffffff", UserId = 1 },
            //    new Person { Id = 11, Name = "Cross Platform", Status = true, ParentId = 6, TextColor = "#000000", BgColor = "#ffffff", UserId = 1 },
            //    new Person { Id = 12, Name = "PHP", Status = true, ParentId = 9, TextColor = "#000000", BgColor = "#ffffff", UserId = 1 },
            //    new Person { Id = 13, Name = "Node.js", Status = true, ParentId = 9, TextColor = "#000000", BgColor = "#ffffff", UserId = 1 },
            //    new Person { Id = 14, Name = ".Net", Status = true, ParentId = 9, TextColor = "#000000", BgColor = "#ffffff", UserId = 1 },
            //    new Person { Id = 15, Name = "Windows Apps", Status = true, ParentId = 10, TextColor = "#000000", BgColor = "#ffffff", UserId = 1 },
            //    new Person { Id = 16, Name = "Store Apps", Status = true, ParentId = 10, TextColor = "#000000", BgColor = "#ffffff", UserId = 1 },
            //    new Person { Id = 17, Name = ".Net Standard", Status = true, ParentId = 14, TextColor = "#000000", BgColor = "#ffffff", UserId = 1 },
            //    new Person { Id = 18, Name = ".Net Core", Status = true, ParentId = 14, TextColor = "#000000", BgColor = "#ffffff", UserId = 1 },
            //    new Person { Id = 19, Name = "Desktop Apps", Status = true, ParentId = 26, TextColor = "#000000", BgColor = "#ffffff", UserId = 1 },
            //    new Person { Id = 20, Name = "Web Apps", Status = true, ParentId = 26, TextColor = "#000000", BgColor = "#ffffff", UserId = 1 },
            //    new Person { Id = 21, Name = "Android Apps", Status = true, ParentId = 26, TextColor = "#000000", BgColor = "#ffffff", UserId = 1 },
            //    new Person { Id = 25, Name = "iOS Apps", Status = true, ParentId = 7, TextColor = "#000000", BgColor = "#ffffff", UserId = 1 },
            //    new Person { Id = 26, Name = "Java", Status = true, ParentId = 9, TextColor = "#000000", BgColor = "#ffffff", UserId = 1 },
            //    new Person { Id = 27, Name = "Xamarin", Status = true, ParentId = 18, TextColor = "#000000", BgColor = "#ffffff", UserId = 1 },
            //    new Person { Id = 28, Name = "Blazor", Status = true, ParentId = 18, TextColor = "#000000", BgColor = "#ffffff", UserId = 1 },
            //    new Person { Id = 29, Name = "MAUI", Status = true, ParentId = 18, TextColor = "#000000", BgColor = "#ffffff", UserId = 1 },
            //    new Person { Id = 30, Name = "Desktop Apps", Status = true, ParentId = 18, TextColor = "#000000", BgColor = "#ffffff", UserId = 1 },
            //    new Person { Id = 31, Name = "Web Apps", Status = true, ParentId = 18, TextColor = "#000000", BgColor = "#ffffff", UserId = 1 },
            //    new Person { Id = 32, Name = "Desktop Apps", Status = true, ParentId = 17, TextColor = "#000000", BgColor = "#ffffff", UserId = 1 },
            //    new Person { Id = 33, Name = "Web Apps", Status = true, ParentId = 17, TextColor = "#000000", BgColor = "#ffffff", UserId = 1 }
            //);

            base.OnModelCreating(modelBuilder);
        }
    }
}
