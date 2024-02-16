using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TreeViewDemo.Models;

namespace TreeViewDemo.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<TreeViewDemo.Models.Category> Categories { get; set; } = default!;
    }
}
