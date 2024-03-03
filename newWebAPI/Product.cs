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
