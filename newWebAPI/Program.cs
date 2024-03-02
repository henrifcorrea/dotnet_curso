using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

/// Ultilizando o GET para extrair (via rota) informações da nossa base de dados (Classe)
app.MapGet("/products/{code}", ([FromRoute] string code) => {
    var product = ProductRepository.GetBy(code);
    return product;
});

/// Método POST
/// Utilizando o POST para armazenar informações na nossa base de dados (Classe)
app.MapPost("/products", (Product product) => {
    ProductRepository.Add(product);
});

/// Método PUT
/// Atualizar um dado na nossa base de dados (Classe)
app.MapPut("/products", (Product product) => {
    var productSaved = ProductRepository.GetBy(product.Code);
    productSaved.Name = product.Name;
});

/// Método DELETE
/// Deletar (por rota) um dado na nossa base de dados (Classe)
app.MapDelete("/products/{code}", ([FromRoute] string code) => {
    var productSaved = ProductRepository.GetBy(code);
    ProductRepository.Remove(productSaved);
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