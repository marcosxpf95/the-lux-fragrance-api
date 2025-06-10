using Microsoft.EntityFrameworkCore;
using the_lux_fragrance_api.Data;
using the_lux_fragrance_api.Helpers;
using the_lux_fragrance_api.Repository;
using the_lux_fragrance_api.Repository.Interface;
using the_lux_fragrance_api.Service;
using the_lux_fragrance_api.Service.Interface;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var IsDevelopment = Environment
                    .GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

var connectionString = IsDevelopment ?
      builder.Configuration.GetConnectionString("DefaultConnection") :
      GetHerokuConnectionString();

builder.Services.AddDbContext<CatalogoContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))); // Usando PostgreSQL

builder.Services.AddControllersWithViews();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IItemService, ItemService>();
builder.Services.AddScoped<IItemRepository, ItemRepository>();
builder.Services.AddScoped<IVendedorService, VendedorService>();
builder.Services.AddScoped<IVendedorRepository, VendedorRepository>();
builder.Services.AddScoped<ICatalogoService, CatalogoService>();
builder.Services.AddScoped<ICatalogoRepository, CatalogoRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<JwtService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseCors(options => options
    .WithOrigins(
        "http://localhost:3000",
        "http://127.0.0.1:5173",
        "http://localhost:4200"
    )
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials()
);

app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
        options.RoutePrefix = string.Empty;  // Configura o Swagger UI para estar na raiz da aplicação
    });
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
app.Urls.Add($"http://*:{port}");

app.Run();


static string GetHerokuConnectionString()
{
    string connectionUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
    var databaseUri = new Uri(connectionUrl);

    string db = databaseUri.LocalPath.TrimStart('/');

    string[] userInfo = databaseUri.UserInfo
                        .Split(':', StringSplitOptions.RemoveEmptyEntries);

    return $"User ID={userInfo[0]};Password={userInfo[1]};Host={databaseUri.Host};" +
           $"Port={databaseUri.Port};Database={db};Pooling=true;" +
           $"SSL Mode=Require;Trust Server Certificate=True;";
}