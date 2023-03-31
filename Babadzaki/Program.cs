using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.HttpsPolicy;
using Babadzaki.Data;
using Microsoft.AspNetCore.Identity;
using Babadzaki_Utility;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages().AddRazorRuntimeCompilation();//компиляция razor страниц в рантайме

// Add services to the container.

builder.Services.AddTransient<IMailService,CustomMailService>();
builder.Services.AddControllersWithViews()
     .AddNewtonsoftJson(options =>//МБ удалить
    {
        options.SerializerSettings.ContractResolver = new DefaultContractResolver();
    });


builder.Services.AddHttpContextAccessor(); //добавление сессий
builder.Services.AddSession(options => //добавление сессий
{ 
    options.IdleTimeout = TimeSpan.FromMinutes(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")));
//builder.Services.AddIdentity<IdentityUser,IdentityRole>()//Добавление системы идентификации
//    .AddDefaultTokenProviders()//предоставляет токены по умолчанию(например если пароль будет утерян)
//    .AddDefaultUI()
//    .AddEntityFrameworkStores<ApplicationDbContext>();
    
    
    

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
//TODO:!