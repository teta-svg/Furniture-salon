using FurnitureSalon.Api.Data;
using FurnitureSalon.Api.Interfaces;
using FurnitureSalon.Api.Repositories;
using FurnitureSalon.Api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<FurnitureSalonDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("FurnitureSalon")));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();