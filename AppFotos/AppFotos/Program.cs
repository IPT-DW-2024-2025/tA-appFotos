using AppFotos.Data;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// ler do ficheiro 'appsettings.json' os dados da BD
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
// define o tipo de BD e a sua 'ligação'
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();



builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();


// configurar o de uso de 'cookies'
builder.Services.AddSession(options => {
   options.IdleTimeout = TimeSpan.FromSeconds(60);
   options.Cookie.HttpOnly = true;
   options.Cookie.IsEssential = true;
} );
builder.Services.AddDistributedMemoryCache();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
   app.UseMigrationsEndPoint();
}
else {
   app.UseExceptionHandler("/Home/Error");
   // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
   app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// começar a usar, realmente, os 'cookies'
app.UseSession();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
