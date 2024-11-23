using EstoqueProdutos.Models;

public class ProdutoVendaModel
{
    public int Id { get; set; }
    public int VendaId { get; set; }
    public VendaModel Venda { get; set; }
    public int ProdutoId { get; set; }
    public ProdutoModel Produto { get; set; }
    public string NomeProduto { get; set; }
    public int Quantidade { get; set; }
    public double ValorProduto { get; set; }
}