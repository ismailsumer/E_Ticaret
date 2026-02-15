using ETicaretAPI.Data;
using ETicaretAPI.Services;
using ETicaretAPI.Mapping;
using System.Text;
using ETicaretAPI.Middlewares;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ETicaretAPI.Repositories;

using Serilog;

using FluentValidation.AspNetCore;
using FluentValidation;
using ETicaretAPI.Validators; // Ensure namespace is correct

var builder = WebApplication.CreateBuilder(args);

// FluentValidation Register
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<ProductCreateDtoValidator>(); // Register all validators in assembly

// Serilog Yapılandırması
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information() // Sadece önemli bilgileri ve hataları tut
    .WriteTo.Console() // Konsola yaz
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day) // Dosyaya günlük yaz
    .CreateLogger();

builder.Host.UseSerilog(); // .NET'e Serilog kullanmasını söyle 



builder.Services.AddDbContext<ETicaretContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))); 

    builder.Services.AddAutoMapper(typeof(GeneralMapping)); 
    builder.Services.AddScoped<IAuthService, AuthService>(); 

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "ETicaretAPI", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference { Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            new string[] { }
        }
    });
}); // CORS Politikası Tanımlama
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()   // Tüm adreslere izin ver
                   .AllowAnyMethod()   // Tüm metodlara (GET, POST, PUT, DELETE) izin ver
                   .AllowAnyHeader();  // Tüm başlıklara izin ver
        });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IBasketService, BasketService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<IBrandService, BrandService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IShippingService, ShippingService>();
builder.Services.AddScoped<ISliderService, SliderService>();
builder.Services.AddScoped<ISettingsService, SettingsService>();

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors("AllowAll"); // CORS politikasını en başta devreye al (preflight istekleri için)
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
