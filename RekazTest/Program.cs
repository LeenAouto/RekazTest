using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RekazTest.Abstractions;
using RekazTest.DatabaseAccess;
using RekazTest.Services;
using RekazTest.Services.StrategyPattern;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

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


//builder.Configuration.AddEnvironmentVariables();


//var awsAccessKey = builder.Configuration["S3Options:AccessKey"];
//var awsSecretKey = builder.Configuration["S3Options:SecretKey"];
//var awsRegion = builder.Configuration["S3Options:Region"];
//var awsBucket = builder.Configuration["S3Options:BlobsBucketName"];

//var apiKey = Environment.GetEnvironmentVariable("AWS_BUCKET_RKZ");






// Register your AmazonS3 storage backend service
//builder.Services.AddSingleton<IStorageBackend>(sp =>
//    new AmazonS3(
//        awsBucket,
//        awsRegion,
//        awsAccessKey,
//        awsSecretKey
//    ));

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

app.MapGet("/config", (IConfiguration config) =>
{
    var blobsBucketName = config["S3Options:BlobsBucketName"];
    var region = config["S3Options:Region"];
    var accessKey = config["S3Options:AccessKey"];
    var secretKey = config["S3Options:SecretKey"];

    return $"BlobsBucketName: {blobsBucketName}, Region: {region}, AccessKey: {accessKey}, SecretKey: {secretKey}";
});

app.Run();
