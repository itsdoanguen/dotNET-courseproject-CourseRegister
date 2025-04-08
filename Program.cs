using DotNetEnv;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
Env.Load();

// Add services to the container.
builder.Configuration.AddEnvironmentVariables();
builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration["DB_CONNECTION_STRING"];
builder.Services.AddDbContext<dotNET_courseproject_CourseRegister.Data.ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));



var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
