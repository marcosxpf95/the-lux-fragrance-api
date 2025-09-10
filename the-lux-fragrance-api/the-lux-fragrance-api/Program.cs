using Microsoft.EntityFrameworkCore;
using the_lux_fragrance_api.Data;
using the_lux_fragrance_api.Helpers;
using the_lux_fragrance_api.Repository;
using the_lux_fragrance_api.Repository.Interface;
using the_lux_fragrance_api.Service;
using the_lux_fragrance_api.Service.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer; // Added for JWT Bearer authentication
using Microsoft.IdentityModel.Tokens; // Added for SymmetricSecurityKey
using System.Text; // Added for Encoding
using Microsoft.OpenApi.Models; // Added for OpenApiSecurityScheme and OpenApiSecurityRequirement

var builder = WebApplication.CreateBuilder(args);

// Configure CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("SpecificOrigins", builder =>
    {
        builder.WithOrigins(
                "http://localhost:3000",
                "http://127.0.0.1:5173",
                "http://localhost:4200",
                "http://theluxfragrance.com.br",
                "https://theluxfragrance.com.br",
                "http://www.theluxfragrance.com.br",
                "https://www.theluxfragrance.com.br"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

// Configure Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true, // Validate the signature of the token
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"])), // The secret key used to sign the token
            ValidateIssuer = true, // Validate the issuer of the token
            ValidIssuer = builder.Configuration["Jwt:Issuer"], // The expected issuer of the token (e.g., your API's URL)
            ValidateAudience = false, // Do not validate the audience (set to true and provide ValidAudience if needed)
            ValidateLifetime = true, // Validate the token's expiration date
            ClockSkew = TimeSpan.Zero // No leeway for expiration time
        };
    });

builder.Services.AddAuthorization(); // Add authorization services

var IsDevelopment = Environment
                    .GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

var connectionString = IsDevelopment ?
      builder.Configuration.GetConnectionString("DefaultConnection") :
      GetHerokuConnectionString();

builder.Services.AddDbContext<CatalogoContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddControllersWithViews();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "The Lux Fragrance API", Version = "v1" });
    
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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

// Register services and repositories for Dependency Injection
builder.Services.AddServices(builder.Configuration);
builder.Services.AddRepositories(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Apply the specific CORS policy
app.UseCors("SpecificOrigins"); // Only one app.UseCors call, applying the named policy
app.UseHttpsRedirection(); // Redirect HTTP requests to HTTPS
app.UseRouting(); // Enable routing
app.UseAuthentication(); // IMPORTANT: This must come before UseAuthorization()
app.UseAuthorization(); // Enable authorization

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Enable Swagger UI in development
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
        options.RoutePrefix = string.Empty;  // Configura o Swagger UI para estar na raiz da aplicação
    });
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Configure the application to listen on the port specified by Heroku or default to 5000
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
app.Urls.Add($"http://*:{port}");

app.Run();

// Helper method to get PostgreSQL connection string for Heroku
static string GetHerokuConnectionString()
{
    string connectionUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
    if (string.IsNullOrEmpty(connectionUrl))
    {
        throw new InvalidOperationException("DATABASE_URL environment variable is not set.");
    }

    var databaseUri = new Uri(connectionUrl);

    string db = databaseUri.LocalPath.TrimStart('/');

    string[] userInfo = databaseUri.UserInfo
                        .Split(':', StringSplitOptions.RemoveEmptyEntries);

    return $"User ID={userInfo[0]};Password={userInfo[1]};Host={databaseUri.Host};" +
           $"Port={databaseUri.Port};Database={db};Pooling=true;" +
           $"SSL Mode=Require;Trust Server Certificate=True;";
}
