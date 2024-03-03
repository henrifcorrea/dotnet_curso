using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

var builder = WebApplication.CreateBuilder(args);

/// Tornando a classe de conexão do banco de dados em um serviço do AspNet para ser utilizado no código.
builder.Services.AddSqlServer<ApplicationDbContext>(builder.Configuration["Database:SqlServer"]);

var app = builder.Build();
var configuration = app.Configuration;
ProductRepository.Init(configuration);

/// Método POST
/// Utilizando o POST para armazenar informações na nossa base de dados e adicionando um retorno de status code de criação (201).
app.MapPost("/products", (ProductRequest productRequest, ApplicationDbContext context) => {
    var category = context.Categories.Where(c => c.Id == productRequest.CategoryId).First();
    var product = new Product {
        Code = productRequest.Code,
        Name = productRequest.Name,
        Description = productRequest.Description,
        Catgory = category
    };
    if(productRequest.Tags != null){
        product.Tags = new List<Tag>();
        foreach (var item in productRequest.Tags){
            product.Tags.Add(new Tag{ Name = item });
        }
    }
    context.Products.Add(product);
    context.SaveChanges();
    return Results.Created($"/products/{product.Id}", product.Id);
});

/// Método GET
/// Ultilizando o GET para extrair (via rota) informações da nossa base de dados e adicionando uma condição de retorno para caso o código do cliente buscado não exista (404).
app.MapGet("/products/{id}", ([FromRoute] int id, ApplicationDbContext context) => {
    var product = context.Products
    .Include(p => p.Catgory)
    .Include(p => p.Tags)
    .Where(p => p.Id == id).First();
    if(product != null){
        return Results.Ok(product);
    }
    return Results.NotFound();
});

/// Método PUT
/// Atualizar um dado na nossa base de dados (Classe).
app.MapPut("/products/{id}", ([FromRoute] int id, ProductRequest productRequest, ApplicationDbContext context) => {
    var product = context.Products
    .Include(p => p.Tags)
    .Where(p => p.Id == id).First();
    var category = context.Categories.Where(c => c.Id == productRequest.CategoryId).First();

    product.Code = productRequest.Code;
    product.Name = productRequest.Name;
    product.Description = productRequest.Description;
    product.Catgory = category;
    product.Tags = new List<Tag>();

    if(productRequest.Tags != null){
        product.Tags = new List<Tag>();
        foreach (var item in productRequest.Tags){
            product.Tags.Add(new Tag{ Name = item });
        }
    }
    context.SaveChanges();
    return Results.Ok();
});

/// Método DELETE
/// Deletar (por rota) um dado na nossa base de dados.
app.MapDelete("/products/{id}", ([FromRoute] int id, ApplicationDbContext context) => {
    var product = context.Products.Where(p => p.Id == id).First();
    context.Products.Remove(product);
    context.SaveChanges();
    return Results.Ok();
});

app.Run();