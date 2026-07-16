using System.Text;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;
using KRecaWebAPI.BackgroundServices;
using KRecaWebAPI.Middlewares;
using KReca.Business.Interfaces;
using KReca.Business.Services;
using KReca.Data;
using KReca.Data.Interfaces;
using KReca.Data.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration
        .GetConnectionString("DefaultConnection")));

// Repositories
builder.Services.AddScoped<IUrunRepository, UrunRepository>();
builder.Services.AddScoped<ISiparisRepository, SiparisRepository>();
builder.Services.AddScoped<IKategoriRepository, KategoriRepository>();
builder.Services.AddScoped<ISiteAyarlariRepository, SiteAyarlariRepository>();
builder.Services.AddScoped<IIletisimMesajiRepository, IletisimMesajiRepository>();

// Services
builder.Services.AddScoped<ISiteAyarlariService, SiteAyarlariService>();
builder.Services.AddScoped<IUrunService, UrunService>();
builder.Services.AddScoped<ISiparisService, SiparisService>();
builder.Services.AddScoped<IKategoriService, KategoriService>();
builder.Services.AddScoped<IPayTRService, PayTRService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IResimService, ResimService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IIletisimMesajiService, IletisimMesajiService>();

// Background Services
builder.Services.AddHostedService<SiparisTemizlemeService>();

// JWT
var jwtSecret = builder.Configuration["JWT:Secret"]!;
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidAudience = builder.Configuration["JWT:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSecret))
        };
    });

builder.Services.AddAuthorization();

// CORS — Production'da sadece frontend origin'ine izin ver
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
            ?? new[] { "http://localhost:5173", "https://localhost:5173" };
        
        policy.WithOrigins(allowedOrigins)
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Rate Limiting (Brute Force ve Spam Koruması)
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("LoginPolicy", opt =>
    {
        opt.Window = TimeSpan.FromMinutes(1);
        opt.PermitLimit = 5; // Dakikada 5 istek
        opt.QueueLimit = 0;
    });

    options.AddFixedWindowLimiter("OrderPolicy", opt =>
    {
        opt.Window = TimeSpan.FromMinutes(1);
        opt.PermitLimit = 10; // Dakikada 10 sipariş oluşturma
        opt.QueueLimit = 0;
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Global Exception Handler
app.UseMiddleware<ExceptionMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("AllowFrontend");

// Rate Limiting Middleware
app.UseRateLimiter();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();