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

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Usuario/Login";
        options.AccessDeniedPath = "/Usuario/AccessDenied"; // 👈 importante que coincida con el controlador real
    });


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdministradorOnly", policy =>
        policy.RequireClaim(ClaimTypes.Role, "Administrador"));
});


var key = Encoding.ASCII.GetBytes("clave-secreta-super-segura"); // Usá una más fuerte en prod

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false; // Solo para testing local
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

var app = builder.Build(); // 🔴 DESPUÉS de registrar servicios





// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
