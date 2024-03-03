using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

/// Tornando a classe de conexão do banco de dados em um serviço do AspNet para ser utilizado no código.
builder.Services.AddDbContext<ApplicationDbContext>();

var app = builder.Build();
var configuration = app.Configuration;
ProductRepository.Init(configuration);

/// Método POST
/// Utilizando o POST para armazenar informações na nossa base de dados (Classe) e adicionando um retorno de status code de criação (201).
app.MapPost("/products", (Product product) => {
    ProductRepository.Add(product);
    return Results.Created($"/products/{product.Code}", product.Code);
});

/// Método GET
/// Ultilizando o GET para extrair (via rota) informações da nossa base de dados (Classe) e adicionando uma condição de retorno para caso o código do cliente buscado não exista (404).
app.MapGet("/products/{code}", ([FromRoute] string code) => {
    var product = ProductRepository.GetBy(code);
    if(product != null)
        return Results.Ok(product);
    return Results.NotFound();
});

/// Criando um endpoint que retorne o nome do database pegando do arquivo de configuration usando uma condição para definir qual vai ser o ambiente que será possivel pegar a informação.
if(app.Environment.IsStaging())
    app.MapGet("/configuration/database", (IConfiguration config) => {
        return Results.Ok($"{config["Database:Connection"]}:{config["Database:Port"]}");
    });

/// Método PUT
/// Atualizar um dado na nossa base de dados (Classe).
app.MapPut("/products", (Product product) => {
    var productSaved = ProductRepository.GetBy(product.Code);
    productSaved.Name = product.Name;
    return Results.Ok();
});

/// Método DELETE
/// Deletar (por rota) um dado na nossa base de dados (Classe).
app.MapDelete("/products/{code}", ([FromRoute] string code) => {
    var productSaved = ProductRepository.GetBy(code);
    ProductRepository.Remove(productSaved);
    return Results.Ok();
});

app.Run();

/// Criando uma classe para armazenar os dados (Simulando um DB)
public static class ProductRepository {
    public static List<Product> Products { get; set; } = Products = new List<Product>();
    public static void Init(IConfiguration config) {
        var products = config.GetSection("Products").Get<List<Product>>();
        Products = products;
    }
    public static void Add(Product product) {
        Products.Add(product);
    }
    public static Product GetBy(string code) {
        return Products.FirstOrDefault(p => p.Code == code);
    }
    public static void Remove(Product product) {
        Products.Remove(product);
    }
}

/// Criando uma classe com as "tabelas" referente as categorias.
public class Catgory {
    public int Id { get; set; }
    public string Name { get; set; }

}

/// Criando uma classe com as "tabelas" referente as Tags.
public class Tag {
    public int Id { get; set; }
    public string Name { get; set; }
    public int ProductId { get; set; }
}

/// Criando uma classe com as "tabelas" referente ao produto.
public class Product {
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int CatgoryId { get; set; }
    public Catgory Catgory { get; set; }
    public List<Tag> Tags { get; set; }
}

/// Criando uma classe para configurar nosso banco de dados e posteriormente se tornar uma tabela.
public class ApplicationDbContext : DbContext {
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Product>()
            .Property(p => p.Description).HasMaxLength(500).IsRequired(false);
        builder.Entity<Product>()
            .Property(p => p.Name).HasMaxLength(120).IsRequired();
        builder.Entity<Product>()
            .Property(p => p.Code).HasMaxLength(20).IsRequired();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlServer("Server=localhost;Database=Products;User Id=sa;Password=@sql2019;MultipleActiveResultSets=true;Encrypt=YES;TrustServerCertificate=YES");
}