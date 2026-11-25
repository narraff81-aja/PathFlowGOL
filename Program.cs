using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PathFlowGOL.Data;
using PathFlowGOL.Models;
using PathFlowGOL.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 1. Configurazione MySQL con EF Core (Pomelo)
var Server = builder.Configuration["DatabaseConfig:Server"];
var Port = builder.Configuration["DatabaseConfig:Port"];
var DBName = builder.Configuration["DatabaseConfig:DBName"];
var UserID = builder.Configuration["DatabaseConfig:UserID"];
var Password = builder.Configuration["DatabaseConfig:Password"];
var mySqlConnectionString = builder.Configuration.GetConnectionString("MySqlDb") ?? "Server={0};Port={1};Database={2};Uid={3};Pwd={4};";
mySqlConnectionString = string.Format(mySqlConnectionString, Server, Port, DBName, UserID, Password);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(mySqlConnectionString, ServerVersion.AutoDetect(mySqlConnectionString))
);
// Registro l'hasher per le password nel container DI
builder.Services.AddScoped<IPasswordHasher<Utente>, PasswordHasher<Utente>>();
builder.Services.AddScoped<ICorsoService, CorsoService>();
builder.Services.AddScoped<ILavoroObiettivoService, LavoroObiettivoService>();
builder.Services.AddScoped<IStatoInterventoService, StatoInterventoService>();
builder.Services.AddScoped<ITipoBeneficioService, TipoBeneficioService>();
builder.Services.AddScoped<ITitoloStudioService, TitoloStudioService>();

// Configurazione dell'Autenticazione basata su Cookie
builder.Services.AddAuthentication(Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options => {
        options.LoginPath = "/api/auth/login"; // Rotta di reindirizzamento se non autorizzato
        options.ExpireTimeSpan = TimeSpan.FromHours(8); // Durata della sessione
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();