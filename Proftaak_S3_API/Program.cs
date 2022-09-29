using Microsoft.EntityFrameworkCore;
using Proftaak_S3_API.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<ProftaakContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DBConnection")));
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(x => x
                .WithOrigins("http://localhost:3000")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
