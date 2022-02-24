using NETCoreProvisionB2CUsersAPI.Interfaces;
using NETCoreProvisionB2CUsersAPI.Models;
using NETCoreProvisionB2CUsersAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "CORS",
        builder => builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
}
);

builder.Services.AddScoped<IB2CUser, B2CUser>();
builder.Services.Configure<B2CTenantSettings>(builder.Configuration.GetSection("AzureADB2C"));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors("CORS");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
