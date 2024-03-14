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
        public DbSet<Category> Categories { get; init; } = default!;
        public DbSet<AppUser> AppUsers { get; init; } = default!;
        public DbSet<AppUserLoginHistory> AppUserLoginHistories { get; set; }
        
    }
}
