using EventoCore.Context;
using EventoCore.Entities;
using EventoCore.Interfaces;
using EventoCore.Repositories;
using EventoCore.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IEventoRepository, EventoRepository>();
builder.Services.AddScoped<IConviteRepository, ConviteRepository>();

builder.Services.AddDbContext<ApplicationContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});


////builder.Services.AddIdentity<Usuario, IdentityRole<Guid>>()
////    .AddEntityFrameworkStores<ApplicationContext>()
////    .AddDefaultTokenProviders();

////builder.Services.ConfigureApplicationCookie(options =>
////{
////    options.LoginPath = new PathString("/Login");
////});

//var jwtSettingsSection = builder.Configuration.GetSection("JwtSettings");
//builder.Services.Configure<JwtSettings>(jwtSettingsSection);
//var jwtSettings = jwtSettingsSection.Get<JwtSettings>();
//var key = Encoding.ASCII.GetBytes(jwtSettings.Segredo);

// Configuração do JWT
//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//.AddCookie(options =>
//{
//    options.Events = new CookieAuthenticationEvents
//    {
//        OnRedirectToLogin = ctx =>
//        {
//            ctx.Response.Redirect("/");
//            return Task.CompletedTask;
//        }
//    };
//})
//.AddJwtBearer(options =>
//{
//    options.SaveToken = true;
//    options.Events = new JwtBearerEvents
//    {
//        OnMessageReceived = context =>
//        {
//            if (context.Request.Cookies.ContainsKey("UserAuthToken"))
//            {
//                context.Token = context.Request.Cookies["UserAuthToken"];
//            }
//            return Task.CompletedTask;
//        }
//    };
//});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/"; // Caminho para a sua página de login
        options.AccessDeniedPath = "/"; // Página para acesso negado
        // Configurações adicionais conforme necessário
    });


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


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Autenticacao}/{action=Login}/{id?}");

app.Run();
