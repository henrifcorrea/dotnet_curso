using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

/// Tornando a classe de conexão do banco de dados em um serviço do AspNet para ser utilizado no código.
builder.Services.AddSqlServer<ApplicationDbContext>(builder.Configuration["Database:SqlServer"]);

var app = builder.Build();
var configuration = app.Configuration;
ProductRepository.Init(configuration);

/// Método POST
/// Utilizando o POST para armazenar informações na nossa base de dados (Classe) e adicionando um retorno de status code de criação (201).
app.MapPost("/products", (ProductRequest productRequest, ApplicationDbContext context) => {
    var category = context.Categories.Where(c => c.Id == productRequest.CategoryId).First();
    var product = new Product {
        Code = productRequest.Code,
        Name = productRequest.Name,
        Description = productRequest.Description,
        Catgory = category
    };
    context.Products.Add(product);
    context.SaveChanges();
    return Results.Created($"/products/{product.Id}", product.Id);
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