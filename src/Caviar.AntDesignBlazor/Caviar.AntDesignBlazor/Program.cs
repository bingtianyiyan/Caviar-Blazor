using Caviar.AntDesignUI;
using Caviar.Infrastructure;
using Caviar.AntDesignBlazor.Components;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Caviar.Core.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

//�����֤����
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;

    options.Lockout.AllowedForNewUsers = false;
    options.Lockout.MaxFailedAccessAttempts = 10;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);

    options.User.RequireUniqueEmail = true;
});
//cookies����
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
});
//cookies��֤����
builder.Services
    .AddAuthentication(cfg =>
    {
        cfg.DefaultScheme = IdentityConstants.ApplicationScheme;
        cfg.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer()
.AddCookie();

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.MinimumSameSitePolicy = SameSiteMode.None;
    options.Secure = CookieSecurePolicy.Always;
});

// Add services to the container.
//builder.Services.AddRazorPages();   //cshtmlҳ�棬razor ҳ��

Caviar.AntDesignBlazor.Client.Program.PublicInit();

//�ͻ�������
builder.Services.AddCaviarServer();
builder.Services.AddAdminCaviar(new Type[] { typeof(Program), typeof(Caviar.AntDesignBlazor.Client._Imports) });

//���������
builder.Services.AddCaviar();
builder.Services.AddCaviarDbContext(options =>

options.UseMySql(
builder.Configuration.GetConnectionString("DefaultConnection"), ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
, b => b.MigrationsAssembly("Caviar.AntDesignBlazor")));
//����
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});
//������
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseBlazorFrameworkFiles();

app.UseStaticFiles();
app.UseRouting();
app.UseCaviar();

app.UseHttpsRedirection();

app.UseAntiforgery();

//app.MapRazorPages();      //cshtmlҳ�棬razor ҳ��
app.MapControllers();
//app.MapBlazorHub();       //�������� Blazor �� SignalR Hub �ս��
//app.MapFallbackToPage("_Host");

app.MapRazorComponents<Caviar.AntDesignBlazor.Components.App>()     //MapRazorComponents ����razor���
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Caviar.AntDesignBlazor.Client._Imports).Assembly, typeof(Caviar.AntDesignUI._Imports).Assembly);

app.Run();
