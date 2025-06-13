using Microsoft.EntityFrameworkCore;
using System.Text;
using WebAPI.Data;
using WebAPI.Logic;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowUI", policy =>
    {
        policy.WithOrigins("https://localhost:7224") // UI projesinin adresi
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

//dependency injection
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("FotografAppDb"),
        new MySqlServerVersion(new Version(8,0,0))
));

var app = builder.Build();
app.UseStaticFiles(); // wwwroot klasöründen eriþim saðlar

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowUI");
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
