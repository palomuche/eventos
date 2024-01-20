using EventoCore.Context;
using EventoCore.Entities;
using EventoCore.Interfaces;
using EventoCore.Repositories;
using EventoCore.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IEventoRepository, EventoRepository>();

builder.Services.AddDbContext<ApplicationContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});


builder.Services.AddIdentity<Usuario, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<ApplicationContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = new PathString("/Login");
});

//// Add authentication services
//builder.Services.AddAuthentication("YourCookieAuthenticationScheme")
//    .AddCookie("YourCookieAuthenticationScheme", options =>
//    {
//        options.LoginPath = new PathString("/Login"); // or your login path
//        options.AccessDeniedPath = "/Autenticacao/AccessDenied"; // or your access denied path
//    });


//var jwtSettingsSection = builder.Configuration.GetSection("JwtSettings");
//builder.Services.Configure<JwtSettings>(jwtSettingsSection);
//var jwtSettings = jwtSettingsSection.Get<JwtSettings>();
//var key = Encoding.ASCII.GetBytes(jwtSettings.Segredo);

//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//}).AddJwtBearer(options =>
//{
//    options.RequireHttpsMetadata = true;
//    options.SaveToken = true;
//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//        IssuerSigningKey = new SymmetricSecurityKey(key),
//        ValidateIssuer = true,
//        ValidateAudience = true,
//        ValidAudience = jwtSettings.Audiencia,
//        ValidIssuer = jwtSettings.Emissor
//    };
//});

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
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
