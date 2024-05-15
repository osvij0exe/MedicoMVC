using MedicoMVC.DataAcces;
using MedicoMVC.Services.Implementacion;
using MedicoMVC.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


var logger= new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);


builder.Services.Configure<SettingStrings>(builder.Configuration.GetSection("ConnectionStrings"));

var aplicationDbContext = builder.Configuration.GetConnectionString("MedicosApplicationDb");
builder.Services.AddDbContext<MedicosApplicationDBContext>(options =>
{
    options.UseSqlServer(aplicationDbContext);
});

//politicas de contraseñas
builder.Services.AddIdentity<MedicoIdentityUser, IdentityRole>(policies =>
{
    policies.Password.RequireDigit = true;
    policies.Password.RequireLowercase = true;
    policies.Password.RequireUppercase = false;
    policies.Password.RequireNonAlphanumeric = false;
    policies.Password.RequiredLength = 6;

    //Todos los usuarios deben ser unicos
    policies.User.RequireUniqueEmail = true;

    //Politicas de bloqueo de cuentas
    policies.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
    policies.Lockout.MaxFailedAccessAttempts = 5;

}).AddEntityFrameworkStores<MedicosApplicationDBContext>()
.AddDefaultTokenProviders();//permite implementar politicas de restablecer la cuenta


builder.Services.AddScoped<IEspecialidadesServices, EspecialidadesServices>();
builder.Services.AddScoped<IMedicoServices, MedicoServices>();


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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
