using FurnitureSalon.Api.Data;
using FurnitureSalon.Api.Interfaces;
using FurnitureSalon.Api.Repositories;
using FurnitureSalon.Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<FurnitureSalonDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("FurnitureSalon")));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<FurnitureSalonDbContext>();
    db.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(options =>
    {
        options.OpenApiVersion = OpenApiSpecVersion.OpenApi2_0;
    });
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Furniture Salon API");
        options.DisplayRequestDuration();
    });
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();