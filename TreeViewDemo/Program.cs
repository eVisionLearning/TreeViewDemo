using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TreeViewDemo;
using TreeViewDemo.Data;
using TreeViewDemo.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AppDbContext") ?? throw new InvalidOperationException("Connection string 'AppDbContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

using var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
try
{
    await context.Database.MigrateAsync();
    if (!context.AppUsers.Any())
    {
        var user = new AppUser
        {
            LoginId = "admin",
            Password = "123456",
            TreeName = "Default Tree"
        };
        context.AppUsers.Add(user);
        await context.SaveChangesAsync();
        user.Password = CoreHandler.GetInstance().Encrypt(user.Password + user.Id);
        await context.SaveChangesAsync();
    }
}
catch (Exception)
{
    // ignored
}

// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
