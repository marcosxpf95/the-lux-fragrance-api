using Microsoft.EntityFrameworkCore;
using the_lux_fragrance_api.Data;

var builder = WebApplication.CreateBuilder(args);

// Configuração do CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()  // Permite qualquer origem
            .AllowAnyMethod()  // Permite qualquer método HTTP
            .AllowAnyHeader(); // Permite qualquer cabeçalho
    });
});

// Configuração do DbContext para conectar ao PostgreSQL
builder.Services.AddDbContext<CatalogoContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))); // Usando PostgreSQL

// Configuração dos controllers
builder.Services.AddControllersWithViews();

// Configuração do Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Se a aplicação não for de desenvolvimento, configure o HSTS e um manipulador de exceções
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();  // HSTS para segurança
}

app.UseCors("AllowAll"); // Ativa o CORS

app.UseHttpsRedirection();  // Redireciona todas as requisições HTTP para HTTPS
app.UseStaticFiles(); // Serve arquivos estáticos (se houver)

app.UseRouting(); // Permite o roteamento dos endpoints da aplicação

app.UseAuthorization(); // Adiciona a autorização se necessário

// Configuração do Swagger para ambiente de desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();  // Gera o Swagger
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
        options.RoutePrefix = string.Empty;  // Configura o Swagger UI para estar na raiz da aplicação
    });
}

// Mapeia as rotas padrão para os controllers
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();