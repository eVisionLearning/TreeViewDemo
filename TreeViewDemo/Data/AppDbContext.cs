using System;
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
        public DbSet<Category> Categories { get; init; }
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
    }
}
