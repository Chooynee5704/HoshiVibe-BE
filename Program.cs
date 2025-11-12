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
using Microsoft.EntityFrameworkCore;
using Npgsql;
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
        policy.WithOrigins(
                "http://localhost:5173",
                "http://localhost:5174", 
                "http://localhost:3000",
                "https://fe-hoshi-vibe.vercel.app",
                "https://hoshivibe-production.up.railway.app",
                "https://*.vercel.app"
              )
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
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
builder.Services.AddScoped<VoucherRepository>();
builder.Services.AddScoped<CharmRepository>();
builder.Services.AddScoped<CustomDesignRepository>();



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
builder.Services.AddScoped<VoucherService>();
builder.Services.AddScoped<CharmService>();
builder.Services.AddScoped<CustomDesignService>();


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
        Description = "JWT Authorization header using the Bearer scheme. Just enter your token in the text input below (no need to type 'Bearer').",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
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
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
var app = builder.Build();

//builder.Services
//    .AddAuthentication()
//    .AddGoogle(options =>
//    {
//        options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
//        options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
//    });



// Configure the HTTP request pipeline.
// Apply migrations on startup
using (var scope = app.Services.CreateScope())
{
    try
    {
        var context = scope.ServiceProvider.GetRequiredService<DataContext>();
        
        Console.WriteLine("Applying database migrations...");
        context.Database.Migrate();
        Console.WriteLine("Database migrations applied successfully ✓");

        // Seed default vouchers if they don't exist
        if (!context.Vouchers.Any())
        {
            Console.WriteLine("Seeding default vouchers...");
            var defaultVouchers = new List<Voucher>
            {
                new Voucher
                {
                    Voucher_Id = Guid.NewGuid(),
                    Code = "HOSHI10",
                    VoucherName = "Giảm giá 10%",
                    DiscountAmount = 0.10m,
                    UseTime = 10,
                    UsedCount = 0,
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow.AddYears(1),
                    IsActive = true
                },
                new Voucher
                {
                    Voucher_Id = Guid.NewGuid(),
                    Code = "HOSHI20",
                    VoucherName = "Giảm giá 20%",
                    DiscountAmount = 0.20m,
                    UseTime = 10,
                    UsedCount = 0,
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow.AddYears(1),
                    IsActive = true
                },
                new Voucher
                {
                    Voucher_Id = Guid.NewGuid(),
                    Code = "HOSHI30",
                    VoucherName = "Giảm giá 30%",
                    DiscountAmount = 0.30m,
                    UseTime = 10,
                    UsedCount = 0,
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow.AddYears(1),
                    IsActive = true
                },
                new Voucher
                {
                    Voucher_Id = Guid.NewGuid(),
                    Code = "HOSHI40",
                    VoucherName = "Giảm giá 40%",
                    DiscountAmount = 0.40m,
                    UseTime = 10,
                    UsedCount = 0,
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow.AddYears(1),
                    IsActive = true
                },
                new Voucher
                {
                    Voucher_Id = Guid.NewGuid(),
                    Code = "HOSHI50",
                    VoucherName = "Giảm giá 50%",
                    DiscountAmount = 0.50m,
                    UseTime = 10,
                    UsedCount = 0,
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow.AddYears(1),
                    IsActive = true
                }
            };

            context.Vouchers.AddRange(defaultVouchers);
            context.SaveChanges();
            Console.WriteLine("Default vouchers seeded successfully ✓");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("=== MIGRATION ERROR ===");
        Console.WriteLine($"Type: {ex.GetType().Name}");
        Console.WriteLine($"Message: {ex.Message}");
        if (ex.InnerException != null)
        {
            Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
        }
    }
}

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
        catch (NpgsqlException npgsqlEx)
        {
            Console.WriteLine("=== NPGSQL EXCEPTION ===");
            Console.WriteLine($"Message: {npgsqlEx.Message}");
            Console.WriteLine($"Error Code: {npgsqlEx.ErrorCode}");
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () => Results.Text("HoshiVibe API is running. Go to /swagger for API")).AllowAnonymous();


app.Run();
