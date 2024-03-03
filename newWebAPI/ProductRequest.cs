///Criando uma classe de representação do payload de requisição para poder salvar um produto. Obs: criar como um public record é mais simples do que como uma classe.
public record ProductRequest(
    string Code, string Name, string Description, int CategoryId, List<string> Tags
);