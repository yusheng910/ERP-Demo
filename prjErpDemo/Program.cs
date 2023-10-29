using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using prjErpDemo.Models;
using prjErpDemo.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// =========== Add DBcontext based on connection string ===========

builder.Services.AddDbContext<db_ErpDemoContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("dbConnection"));
});

// =========== Add Session Service ===========

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Session DueTime
});

// Register CustomAuthorizationFilter here
builder.Services.AddScoped<CustomAuthorizationFilter>();

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

// activate session service before use routing
app.UseSession();

// activate Authentication Middleware
app.UseMiddleware<AuthenticationMiddleware>();
app.UseAuthentication();

app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
