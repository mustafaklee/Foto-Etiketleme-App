using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using WebAPI.Data;
using WebAPI.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowUI", policy =>
//    {
//        policy.WithOrigins("https://localhost:7224") // UI projesinin adresi
//              .AllowAnyHeader()
//              .AllowAnyMethod();
//    });
//});


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost",
        policy => policy
            .WithOrigins("https://localhost:7224")  //frontend url
            .AllowAnyHeader()
            .AllowAnyMethod());
});




//dependency injection
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("FotografAppDb"),
        new MySqlServerVersion(new Version(8, 0, 0))
));

builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("AuthDbContext"),
        new MySqlServerVersion(new Version(8, 0, 0))
));

builder.Services.AddScoped<ITokenRepository, TokenRepository>();

//guvenlik ayarları
builder.Services.AddIdentityCore<IdentityUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AuthDbContext>()
    .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("FotoApp")
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        RoleClaimType = ClaimTypes.Role
    });

var app = builder.Build();

app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//burası allowuı olacak
app.UseCors("AllowLocalhost");
app.UseHttpsRedirection();

// SIRALAMA ÇOK ÖNEMLİ!
app.UseAuthentication(); // ÖNCESİNDE olmalı
app.UseAuthorization();  // SONRASINDA olmalı

app.MapControllers();
app.Run();