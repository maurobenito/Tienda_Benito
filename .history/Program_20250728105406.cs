using Microsoft.EntityFrameworkCore;
using Tienda_Benito.Models;
using Tienda_Benito.Repositorios;
using System.Globalization;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var cultureInfo = new CultureInfo("es-AR");
cultureInfo.NumberFormat.NumberDecimalSeparator = ",";
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

var builder = WebApplication.CreateBuilder(args);

// Leer clave secreta JWT desde appsettings.json
var secretKey = builder.Configuration["Jwt:Key"];
if (string.IsNullOrEmpty(secretKey))
    throw new Exception("Falta configurar Jwt:Key en appsettings.json");

// Servicios
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ProyectoTiendaContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("ProyectoTiendaContext"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("ProyectoTiendaContext"))
    ));

// Repositorios (igual que vos)
builder.Services.AddScoped<IRepositorio<Usuario>, RepositorioUsuario>();
builder.Services.AddTransient<IRepositorio<Producto>, RepositorioProducto>();
builder.Services.AddTransient<IRepositorio<Proveedor>, RepositorioProveedor>();
builder.Services.AddTransient<IRepositorio<Cliente>, RepositorioCliente>();
builder.Services.AddTransient<IRepositorio<Ventum>, RepositorioVenta>();
builder.Services.AddTransient<IRepositorio<Ventadetalle>, RepositorioVentadetalle>();
builder.Services.AddTransient<RepositorioVenta>();

// Autenticación: esquema por defecto COOKIE para UI web
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.LoginPath = "/Usuario/Login";
    options.AccessDeniedPath = "/Usuario/AccessDenied";
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CookieOrJwt", policy =>
    {
        policy.AddAuthenticationSchemes(
            CookieAuthenticationDefaults.AuthenticationScheme,
            JwtBearerDefaults.AuthenticationScheme);
        policy.RequireAuthenticatedUser();
    });
});


var app = builder.Build();

// Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // debe ir antes de Authorization
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
