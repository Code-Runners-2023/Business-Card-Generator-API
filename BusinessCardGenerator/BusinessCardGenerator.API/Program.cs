using BusinessCardGenerator.API.Data;
using BusinessCardGenerator.API.Services;
using BusinessCardGenerator.API.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var connectionString =
    builder.Configuration.GetConnectionString("ApplicationContextConnection") ??
    throw new InvalidOperationException("Connection string 'ApplicationContextConnection' not found.");

var jwtSettings =
    builder.Configuration.GetSection("JwtSettings") ??
    throw new InvalidOperationException("'JwtSettings' not found.");

string jwtIssuer = jwtSettings["Issuer"] ?? throw new InvalidOperationException("JWT Issuer not found!");
string jwtAudience = jwtSettings["Audience"] ?? throw new InvalidOperationException("JWT Audience not found!");
string jwtSecretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey not found!");

string frontendUrl = builder.Configuration.GetValue<string>("FrontendUrl") ??
                     throw new InvalidOperationException("FrontendUrl not found!");

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.WithOrigins(frontendUrl)
                                .AllowAnyHeader()
                                .AllowAnyMethod();
        });
});

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ClockSkew = TimeSpan.Zero,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSecretKey)
            )
        };
    }
);

builder.Services.AddDbContext<ApplicationContext>(c => c.UseNpgsql(connectionString));

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IBusinessCardService, BusinessCardService>();
builder.Services.AddScoped<IDepositService, DepositService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IAzureCloudService, AzureCloudService>();
builder.Services.AddScoped<ITokenService, TokenService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
} else
{
    app.UseExceptionHandler();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseCors();

app.Run();
