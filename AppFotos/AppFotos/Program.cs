using AppFotos.Data;
using AppFotos.Data.Seed;
using AppFotos.Services;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using System.Reflection;

using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// ler do ficheiro 'appsettings.json' os dados da BD
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")??throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
// define o tipo de BD e a sua 'ligação'
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();


// configurar o uso do IdentityUser como 'utilizador' de autenticação
// se não se adicionar à instrução '.AddRoles' não é possível usar os ROLES
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount=true)
   .AddRoles<IdentityRole>()
   .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();


// configurar o de uso de 'cookies'
builder.Services.AddSession(options => {
   options.IdleTimeout=TimeSpan.FromSeconds(60);
   options.Cookie.HttpOnly=true;
   options.Cookie.IsEssential=true;
});
builder.Services.AddDistributedMemoryCache();

// *******************************************************************
// Instalar o package
// Microsoft.AspNetCore.Authentication.JwtBearer
//
// using Microsoft.IdentityModel.Tokens;
// *******************************************************************
// JWT Settings
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

builder.Services.AddAuthentication(options => { })
   .AddCookie("Cookies",options => {
      options.LoginPath="/Identity/Account/Login";
      options.AccessDeniedPath="/Identity/Account/AccessDenied";
   })
   .AddJwtBearer("Bearer",options => {
      options.TokenValidationParameters=new TokenValidationParameters {
         ValidateIssuer=true,
         ValidateAudience=true,
         ValidateLifetime=true,
         ValidateIssuerSigningKey=true,
         ValidIssuer=jwtSettings["Issuer"],
         ValidAudience=jwtSettings["Audience"],
         IssuerSigningKey=new SymmetricSecurityKey(key)
      };
   });


// configuração do JWT
builder.Services.AddScoped<TokenService>();




// Eliminar a proteção de 'ciclos' qd se faz uma pesquisa que envolva um relacionamento 1-N em Linq
// https://code-maze.com/aspnetcore-handling-circular-references-when-working-with-json/
// https://marcionizzola.medium.com/como-resolver-jsonexception-a-possible-object-cycle-was-detected-27e830ea78e5
builder.Services.AddControllers()
                .AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler=ReferenceHandler.IgnoreCycles);






//builder.Services.AddAuthorization(); // Importante para múltiplos esquemas
//builder.Services.AddControllersWithViews();
//builder.Services.AddRazorPages(); // importante para Identity scaffolding (Login, Register, etc.)


// declarar o serviço do Signal R
builder.Services.AddSignalR();


// Eliminar a proteção de 'ciclos' qd se faz uma pesquisa que envolva um relacionamento 1-N em Linq
// https://code-maze.com/aspnetcore-handling-circular-references-when-working-with-json/
// https://marcionizzola.medium.com/como-resolver-jsonexception-a-possible-object-cycle-was-detected-27e830ea78e5
builder.Services.AddControllers()
                .AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler=ReferenceHandler.IgnoreCycles);





// Adiciona o Swagger
// builder.Services.AddEndpointsApiExplorer();   // necessária apenas para APIs mínimas. 
// builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(c => {
   c.SwaggerDoc("v1",new OpenApiInfo {
      Title="Minha API de gestão de Fotos",
      Version="v1",
      Description="API para gestão de categorias, fotografias e utilizadores"
   });

   // Caminho para o XML gerado
   var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
   var xmlPath = Path.Combine(AppContext.BaseDirectory,xmlFile);
   c.IncludeXmlComments(xmlPath);

});


var app = builder.Build();





// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
   app.UseMigrationsEndPoint();

   // Invocar o seed da BD
   app.UseItToSeedSqlServer();

   // iniciar o 'middleware' do Swagger
   app.UseSwagger();
   app.UseSwaggerUI();

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

// cria uma 'ponte' entre o nosso serviço Signal R (o GostosHub)
// e o javascript do browser
app.MapHub<GostosHub>("/gostoshub");


app.Run();
