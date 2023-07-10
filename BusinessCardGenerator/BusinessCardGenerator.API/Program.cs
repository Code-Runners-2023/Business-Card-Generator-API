using BusinessCardGenerator.API.Data;
using BusinessCardGenerator.API.Services;
using BusinessCardGenerator.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString =
    builder.Configuration.GetConnectionString("ApplicationContextConnection") ??
    throw new InvalidOperationException("Connection string 'ApplicationContextConnection' not found.");

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.WithOrigins("https://localhost:5173")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
        });
});

builder.Services.AddDbContext<ApplicationContext>(c => c.UseNpgsql(connectionString));

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IBusinessCardService, BusinessCardService>();
builder.Services.AddScoped<IDepositService, DepositService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IAzureCloudService, AzureCloudService>();

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

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
