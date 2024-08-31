using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Movies.Model;
using Movies.Services;
using System.Security.Cryptography.Xml;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplictionDbContext>(options =>
options.UseSqlServer(connectionString));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddTransient<IGenresService, GenresService>();

builder.Services.AddCors();
builder.Services.AddSwaggerGen(option => 
{
    option.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Title",
        Description = "Description",
       Contact = new OpenApiContact
       {
           Name= "ahmed",
           Email="ahmedmedhat003@yahoo.com",
           Url = new Uri("https://www.google.com")
       }
    });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name="Authorization",
        Type=SecuritySchemeType.ApiKey,
        Scheme= "Bearer",
        BearerFormat="JWT",
        In = ParameterLocation.Header,
        Description = "Enter your JWT"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference{
                    Type=ReferenceType.SecurityScheme,
                    Id= "Bearer"
                },
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<String>()
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
app.UseCors(options => options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseAuthorization();

app.MapControllers();

app.Run();
