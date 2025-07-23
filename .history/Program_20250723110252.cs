using Microsoft.EntityFrameworkCore;
using Tienda_Benito.Models;
using Tienda_Benito.Repositorios;
using System.Globalization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;

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
        options.AccessDeniedPath = "/Home/AccessDenied";
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdministradorOnly", policy =>
        policy.RequireClaim(ClaimTypes.Role, "Administrador"));
});




var app = builder.Build(); // 🔴 DESPUÉS de registrar servicios





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
