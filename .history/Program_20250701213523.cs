using Microsoft.EntityFrameworkCore;
using Tienda_Benito.Models;
using Tienda_Benito.Repositorios;


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
builder.Services.AddTransient<IRepositorio<VentaDetalle>, RepositorioVentaDetalle>();



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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
