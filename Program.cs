using AutoMapper;
using HoshiVibe.DB;
using HoshiVibe.Entities.DTO.ModelRequests.VNPay;
using HoshiVibe.Entities.Models.Base;
using HoshiVibe.Entities.Models.Momo;
using HoshiVibe.Mapper;
using HoshiVibe.Repositories;
using HoshiVibe.Repository;
using HoshiVibe.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDataProtection()
    .SetApplicationName("HoshiVibe");

builder.Services.Configure<VnPayOption>(builder.Configuration.GetSection("VnPay"));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});


builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MappingFile>());

// Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!)),
        RoleClaimType = System.Security.Claims.ClaimTypes.Role
    };
});

// Repositories
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<UserProfileRepository>();
builder.Services.AddScoped<ProductRepository>();
builder.Services.AddScoped<OrderRepository>();
builder.Services.AddScoped<OrderDetailsRepository>();
builder.Services.AddScoped<CartRepository>();
builder.Services.AddScoped<CartItemRepository>();
builder.Services.AddScoped<DestinyRepository>();
builder.Services.AddScoped<ZodiacRepository>();



// PasswordHasher
builder.Services.AddScoped<PasswordHasher<User>>();

// Service
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<UserProfileService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<OrderDetaillsService>();
builder.Services.AddScoped<CartService>();
builder.Services.AddScoped<CartItemsService>();
builder.Services.AddScoped<DashBoardService>();
builder.Services.AddScoped<DestinyService>();
builder.Services.AddScoped<ZodiacService>();
builder.Services.AddScoped<JWTService>();


// Add services to the container.
builder.Services.AddControllers();

// MomoAPI
builder.Services.Configure<MomoModel>(builder.Configuration.GetSection("MomoAPI"));
builder.Services.AddScoped<MomoService>();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen( c => 
{
    // JWT Authentication configuration for Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
var app = builder.Build();

//builder.Services
//    .AddAuthentication()
//    .AddGoogle(options =>
//    {
//        options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
//        options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
//    });



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        try
        {
            var context = scope.ServiceProvider.GetRequiredService<DataContext>();

            Console.WriteLine("Attempting to connect...");
            Console.WriteLine($"Connection String: {builder.Configuration.GetConnectionString("DefaultConnection")}");

            // Thử open connection trực tiếp
            var connection = context.Database.GetDbConnection();
            await connection.OpenAsync();
            Console.WriteLine("Database connection: SUCCESS ✓");
            await connection.CloseAsync();
        }
        catch (SqlException sqlEx)
        {
            Console.WriteLine("=== SQL EXCEPTION ===");
            Console.WriteLine($"Message: {sqlEx.Message}");
            Console.WriteLine($"Error Number: {sqlEx.Number}");
            Console.WriteLine($"State: {sqlEx.State}");
            Console.WriteLine($"Class: {sqlEx.Class}");
            Console.WriteLine($"Server: {sqlEx.Server}");
        }
        catch (Exception ex)
        {
            Console.WriteLine("=== GENERAL EXCEPTION ===");
            Console.WriteLine($"Type: {ex.GetType().Name}");
            Console.WriteLine($"Message: {ex.Message}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
            }
        }
    }
}

app.UseSwagger();
app.UseSwaggerUI();

//app.UseHttpsRedirection();

app.UseCors("AllowAll"); 


app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () => Results.Text("HoshiVibe API is running. Go to /swagger for API")).AllowAnonymous();


app.Run();
