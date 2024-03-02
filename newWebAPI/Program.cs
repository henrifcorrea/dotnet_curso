using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

/// Método GET
app.MapGet("/", () => "Hello World!");

app.MapGet("/User", () => new {Name = "Henrique Correa", Age = 20});

/// Adicionando um header no retorno
app.MapGet("/AddHeader", (HttpResponse response) => {
    response.Headers.Add("Teste", "Teste 2");
    return new {Name = "Henrique Correa", Age = 20};   
});

/// Parametro pela URL (Exemplo: /api.app.com/users?datestart={date}dateend={date})
app.MapGet("/getProduct", ([FromQuery] string dateStart, [FromQuery] string dateEnd) => {
    return dateStart + " - " + dateEnd;
});
/// Atráves de rota na URL (Exemplo: /api.app.com/user/{code})
app.MapGet("/getProduct/{code}", ([FromRoute] string code) => {
    return code;
});

/// Método POST
/// Ultilizando a classe para salvar o prduto.
app.MapPost("/saveProduct", (Product product) => {
    return product.Code + " - " + product.Name;
});

app.Run();

/// Criando uma classe 
public class Product {
    public string Code { get; set; }
    public string Name { get; set; }
}