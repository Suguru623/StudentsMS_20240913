using Microsoft.EntityFrameworkCore;
using StudentsMS.Models;

var builder = WebApplication.CreateBuilder(args);
//1.2.4 在Program.cs內以依賴注入的寫法撰寫讀取連線字串的物件
//      ※注意程式的位置必須要在var builder = WebApplication.CreateBuilder(args);這句之後
builder.Services.AddDbContext<StudentsMSContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("StudentsMSConnection")));



// Add services to the container.
builder.Services.AddControllersWithViews();


//4.2.7 在Program.cs中註冊及啟用Session
//註冊session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

//4.2.7 在Program.cs中註冊及啟用Session
//註冊session
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();


/*
 app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();


//4.2.7 在Program.cs中註冊及啟用Session
//註冊session
app.UseSession();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
 */