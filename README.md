# .NET NEW WEB API<h1>

# Nuget <h3>
**Nuget é um gerenciador de pacote do DOTNET. Caso seja necessário instalar um pacote do DOTNET no seu programa, o Nuget é o unico caminho possível para ser realizado essa instalação**

# Endpoint <h3>
**Endpoint é uma "tabela" virtual onde todos os paths da nossa aplicação pode ser acessada em uma requisição. Cada um desses paths apontam para qual rotina do nosso código será executada**

* GET
*Método para obter uma inforação da API. Ex C#: app.MapGet("/", () => "Hello World!");*

* POST
*Método para inserir uma informação na API. Ex C#: app.MapPost("/saveProduct", (Product product) => {return product.Code + " - " + product.Name;});*

* PUT 
*Método para efeturar alteração na API*

* DELETE
*Método para deletar informação da API*