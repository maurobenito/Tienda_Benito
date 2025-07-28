using Microsoft.EntityFrameworkCore;
using Tienda_Benito.Models;
using Tienda_Benito.Repositorios;
using System.Globalization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var cultureInfo = new CultureInfo("es-AR");
cultureInfo.NumberFormat.NumberDecimalSeparator = ",";
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

var builder = WebApplication.CreateBuilder(args);

// Leer la clave secreta desde appsettings.json
var secretKey = builder.Configuration["Jwt:Key"];
if (string.IsNullOrEmpty(secretKey))
{
    throw new Exception("La clave JWT no está configurada en appsettings.json (Jwt:Key)");
}

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ProyectoTiendaContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("ProyectoTiendaContext"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("ProyectoTiendaContext"))
    ));

// REGISTRO DEL REPOSITORIO
builder.Services.AddScoped<IRepositorio<Usuario>, RepositorioUsuario>();
builder.Services.AddTransient<IRepositorio<Producto>, RepositorioProducto>();
builder.Services.AddTransient<IRepositorio<Proveedor>, RepositorioProveedor>();
builder.Services.AddTransient<IRepositorio<Cliente>, RepositorioCliente>();
builder.Services.AddTransient<IRepositorio<Ventum>, RepositorioVenta>();
builder.Services.AddTransient<IRepositorio<Ventadetalle>, RepositorioVentadetalle>();
builder.Services.AddTransient<RepositorioVenta>(); // esta es extra

// Configuración autenticación JWT + Cookies
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddJwtBearer("Bearer", x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
})
.AddCookie("Cookies", options =>
{
    options.LoginPath = "/Usuario/Login";
    options.AccessDeniedPath = "/Usuario/AccessDenied";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
