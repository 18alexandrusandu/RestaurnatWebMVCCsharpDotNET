using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApplication5.Data;
using WebApplication5.Repositories;
using WebApplication5.Services;
using WebApplication5.Services.implementations;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<RestaurantDbContext>(options =>

    options.UseSqlServer(builder.Configuration.GetConnectionString("WebApplication5Context") ?? throw new InvalidOperationException("Connection string 'WebApplication5Context' not found.")));




// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddTransient<UserRepository>();
builder.Services.AddTransient< MenuRepository >();
builder.Services.AddTransient<ComenziRepository>();
builder.Services.AddScoped< IUnitOfWork, UnitOfWork >();
builder.Services.AddSession();

builder.Services.AddTransient<IComenziService, ComenziService >();
builder.Services.AddTransient<IMenuService, MenuService >();
builder.Services.AddTransient<IUserService,  UserService >();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseExceptionHandler("/Home/Error");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.UseSession();
app.Run();
