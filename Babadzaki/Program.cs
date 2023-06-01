using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.HttpsPolicy;
using Babadzaki.Data;
using Microsoft.AspNetCore.Identity;
using Babadzaki_Utility;
using Newtonsoft.Json.Serialization;
using AutoMapper;
using Newtonsoft.Json;
using Nethereum.Web3;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages().AddRazorRuntimeCompilation();//���������� razor ������� � ��������

// Add services to the container.

builder.Services.AddTransient<IMailService,CustomMailService>();
builder.Services.AddControllersWithViews()
     .AddNewtonsoftJson(options =>//�� �������
    {
        options.SerializerSettings.ContractResolver = new DefaultContractResolver();
        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    });


builder.Services.AddHttpContextAccessor(); //���������� ������
builder.Services.AddSession(options => //���������� ������
{ 
    options.IdleTimeout = TimeSpan.FromMinutes(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddSingleton<Web3>(provider =>
{
    // Configure the Ethereum node URL
    var ethereumUrl = "https://sepolia.infura.io/v3/96eb1666f8a142ed9c21d5e2fa776874";
    return new Web3(ethereumUrl);
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")));
//builder.Services.AddIdentity<IdentityUser,IdentityRole>()//���������� ������� �������������
//    .AddDefaultTokenProviders()//������������� ������ �� ���������(�������� ���� ������ ����� ������)
//    .AddDefaultUI()
//    .AddEntityFrameworkStores<ApplicationDbContext>();


builder.Services.AddAutoMapper(typeof(Program).Assembly);
    

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{   
    
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
//app.UseAuthentication();
//app.UseAuthorization();
app.UseSession();

app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
    


app.Run();

//interface IDefaultUserRole
//{

//    public void SetDefaultUserRole();
//}

//public class DefaultUserRole : IDefaultUserRole
//{
//    private readonly RoleManager<IdentityRole> _roleManager;

//    public DefaultUserRole(RoleManager<IdentityRole> roleManager)
//    {
//        _roleManager = roleManager;
//    }
//    public void SetDefaultUserRole()
//    {

//    }
//}

//TODO:!����������� � ������� �� ����
//TODO:!������������ ����� �������������(�� MVC)
//TODO:!TokenManagementController ���� ���������� ��� ������������(�������������) ������� ������������� � ��������
//TODO:!��������� WebConstants � json
//TODO:!������� �� ������������ � ��� ����� ���������� � � ����� ��������� ���
//TODO:!���� ��� ������������ ��������: EF Core(�������, ������ ���;
//�������� �������� �������������� � �� � ���������� �� ��� ���� ����[��]), Dependency Injection;
//Maping, Routing;
//�������������� � ��������(ContentType etc..);
//�������� ����������� �� �������(������ � �����)
//https://www.youtube.com/@PlatinumTechTalks
//https://metanit.com/sharp/entityframeworkcore/3.3.php
//TODO:!��������� ����� �����������
//TODO:!������� unit �����
//TODO:!��������� ��������� ����� ��� �������� email ��������
//TODO:!������� AutoMapper mb
//TODO:!�������� ��� ������� json ������ ��� ���������� �� �������
//https://stackoverflow.com/questions/40045147/how-to-read-into-memory-the-lines-of-a-text-file-from-an-iformfile-in-asp-net-co?rq=1