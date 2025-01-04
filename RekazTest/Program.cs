using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RekazTest.Abstractions;
using RekazTest.DatabaseAccess;
using RekazTest.Services.StrategyPattern;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


var backendType = builder.Configuration["Backend:Type"];

builder.Services.AddScoped<IStorageBackend>(sp =>
    StorageServiceFactory.Create(
        backendType,
        sp.GetRequiredService<IConfiguration>(),
        sp.GetRequiredService<ApplicationDbContext>()
    ));

builder.Services.AddScoped<StorageServiceContext>();

///////////////////////////////////////////////////////////////////
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
