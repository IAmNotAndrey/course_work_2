using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using MusicSchoolEF.Models.Db;
using MusicSchoolEF.Models.Defaults;
using MusicSchoolEF.Repositories;
using MusicSchoolEF.Repositories.Interfaces;

//// ����������� �����
//var adminRole = new Role();
//adminRole.Name = "admin";
//var teacherRole = new Role();
//teacherRole.Name = "teacher";
//var studentRole = new Role();
//studentRole.Name = "student";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// ��������� ������������
builder.Configuration.AddJsonFile("appsettings.json");


/*
// Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.Cookie.Name = "AuthCookie"; // ��� ������������������ ����
            //options.Cookie.HttpOnly = true;
            options.ExpireTimeSpan = TimeSpan.FromMinutes(60); // ����� ��������� ����� �������� ����
            options.LoginPath = "/Account/Login"; // ���� � �������� �����
            options.AccessDeniedPath = "/Account/AccessDenied"; // ���� � �������� ������ � �������
        });
// Authorization
//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("StudentOnly", policy =>
//    {
//        policy.RequireRole("student");
//    });
//});
builder.Services.AddAuthorization();


builder.Services.AddControllers(options =>
{
    // ��������� ����������� �� ���� ������������ � ��������� �� ���������
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
});
*/

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {

        options.LoginPath = "/Account/Login"; // ���� � �������� �����
        options.AccessDeniedPath = "/Home/Error"; // ���� � �������� � ������� �������
                                                
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // ��������� ����� �������� ����
    });

// ���������� ��������
builder.Services.AddDbContext<Ms2Context>((serviceProvider, options) =>
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("DefaultConnection");
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(5, 7, 24)));
});
builder.Services.AddScoped<IGroupRepository, GroupRepository>();
builder.Services.AddScoped<INodeRepository, NodeRepository>();
builder.Services.AddScoped<IStudentNodeConnectionRepository, StudentNodeConnectionRepository>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

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

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


#region Routing
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");
//app.MapControllerRoute(
//    name: "account",
//    pattern: "{controller=Account}/{action=Login}");
//app.MapControllerRoute(
//    name: "student",
//    pattern: "{controller=Student}/{id:int}/{action=Index}");
//app.MapControllerRoute(
//    name: "teacher",
//    pattern: "{controller=Teacher}/{id:int}/{action=Index}");
#endregion

//using (var db = new Ms2Context())
//{
//    var n = db.Nodes
//        .Include(n => n.StudentNodeConnections)
//        .Single(n => n.Id == 62);
//    Console.WriteLine(n);
//}

app.Run();
