using ExProjetoAPI.Services;
using ExProjetoAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using ExProjetoAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using ExProjetoAPI.Middlewares;

var builder = WebApplication.CreateBuilder(args);

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("http://localhost:5173")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5097); // escuta em 0.0.0.0
});

// Configuração do banco de dados (ajuste a connection string conforme necessário)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configuração do Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders()
    .AddErrorDescriber<IdentityErrorDescriberPtBr>();

// Configuração de opções do Identity
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Password.RequireUppercase = false;
});

// Configuração do JWT
var jwtKey = builder.Configuration["Jwt:Key"];
if (string.IsNullOrEmpty(jwtKey))
{
    throw new InvalidOperationException("A chave JWT não foi configurada. Verifique as configurações em 'appsettings.json'.");
}

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "Bearer";
    options.DefaultChallengeScheme = "Bearer";
    options.DefaultScheme = "Bearer";
})
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtKey))
        };
    });

// Repositories
builder.Services.AddScoped<IUserRefreshTokenRepository, UserRefreshTokenRepository>();

// Services
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddAutoMapper(typeof(ExProjetoAPI.Mapping.MappingProfile));

builder.Services.AddControllers(options =>
{
    // Adiciona autorização global para todos os controladores
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
});

// Desabilita o filtro de validação de modelo padrão do ASP.NET Core
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Insira o token JWT no formato 'Bearer {token}'"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        { 
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(MyAllowSpecificOrigins);
app.UseMiddleware<ExceptionMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }