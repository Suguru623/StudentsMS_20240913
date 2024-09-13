using Microsoft.EntityFrameworkCore;
using StudentsMS.Models;

var builder = WebApplication.CreateBuilder(args);
//1.2.4 �bProgram.cs���H�̿�`�J���g�k���gŪ���s�u�r�ꪺ����
//      ���`�N�{������m�����n�bvar builder = WebApplication.CreateBuilder(args);�o�y����
builder.Services.AddDbContext<StudentsMSContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("StudentsMSConnection")));



// Add services to the container.
builder.Services.AddControllersWithViews();


//4.2.7 �bProgram.cs�����U�αҥ�Session
//���Usession
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

//4.2.7 �bProgram.cs�����U�αҥ�Session
//���Usession
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();


/*
 app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();


//4.2.7 �bProgram.cs�����U�αҥ�Session
//���Usession
app.UseSession();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
 */