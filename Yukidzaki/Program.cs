using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.HttpsPolicy;

using Microsoft.AspNetCore.Identity;
using Babadzaki_Services;
using Newtonsoft.Json.Serialization;
using AutoMapper;
using Newtonsoft.Json;
using Nethereum.Web3;
using Yukidzaki_Services.Implementations;
using Yukidzaki_Services.Interfaces;
using Yukidzaki_DAL.Interfaces;
using Yukidzaki_Domain.Models;
using Yukidzaki_DAL.Repositories;
using Yukidzaki;
using Yukidzaki_DAL;
using System.Net;
using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages().AddRazorRuntimeCompilation();//компиляция razor страниц в рантайме

// Add services to the container.


builder.Services.AddControllersWithViews()
     .AddNewtonsoftJson(options =>//МБ удалить
    {
        options.SerializerSettings.ContractResolver = new DefaultContractResolver();
        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    });


builder.Services.AddHttpContextAccessor(); //добавление сессий
builder.Services.AddSession(options => //добавление сесс1ий
{ 
    options.IdleTimeout = TimeSpan.FromMinutes(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddAntiforgery(x => x.HeaderName = "X-ANTI-FORGERY-TOKEN");
builder.Services.AddSingleton<Web3>(provider =>
{
    // Configure the Ethereum node URL
    var ethereumUrl = "https://sepolia.infura.io/v3/96eb1666f8a142ed9c21d5e2fa776874";
    return new Web3(ethereumUrl);
});

builder.Services.AddDbContext<Yukidzaki_DAL.ApplicationDbContext>(options =>
options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")));
//builder.Services.AddIdentity<IdentityUser,IdentityRole>()//Добавление системы идентификации
//    .AddDefaultTokenProviders()//предоставляет токены по умолчанию(например если пароль будет утерян)
//    .AddDefaultUI()
//    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.InitializeRepositories();
builder.Services.InitializeServices();
builder.Services.AddMemoryCache();
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddAutoMapper(typeof(Yukidzaki_Domain.Mappings.JsonTokenProfile).Assembly);

ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
}
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

//https://github.com/LeoWarp/Automarket
//TODO:!Разобраться с ветками на гите
//TODO:!Оргпнизовать общую маршрутизацию(не MVC)
//TODO:!TokenManagementController один контроллер для менеджемента(администрации) токенов пользователей и фильтров
//TODO:!Перенести WebConstants в json
//TODO:!Удалить не используемое в том числе библиотеки и в целом почистить код
//TODO:!Темы для углубленного изучения: EF Core(ленивая, жадная итд;
//возможно отделить взаимодействие с бд и контроллер на еще один слой[хз]), Dependency Injection;
//Maping, Routing;
//Взаимодействие с клиентом(ContentType etc..);
//Загрузка изображений на клиенте(вместе с темой)
//https://www.youtube.com/@PlatinumTechTalks
//https://metanit.com/sharp/entityframeworkcore/3.3.php
//TODO:!Настроить общее логирование
//TODO:!Сделать unit тесты
//TODO:!Затестить Фабричный метод для отправки email рассылки
//TODO:!Удалить AutoMapper mb
//TODO:!Подумать над чтением json файлов без сохранения на сервере
//https://stackoverflow.com/questions/40045147/how-to-read-into-memory-the-lines-of-a-text-file-from-an-iformfile-in-asp-net-co?rq=1
//AsNoTracking