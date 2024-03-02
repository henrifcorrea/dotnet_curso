using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

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
    public static List<Product> Products { get; set; }

    public static void Add(Product product) {
        if(Products == null)
            Products = new List<Product>();

        Products.Add(product);
    }

    public static Product GetBy(string code) {
        return Products.FirstOrDefault(p => p.Code == code);
    }

    public static void Remove(Product product) {
        Products.Remove(product);
    }
}

/// Criando uma classe 
public class Product {
    public string Code { get; set; }
    public string Name { get; set; }
}