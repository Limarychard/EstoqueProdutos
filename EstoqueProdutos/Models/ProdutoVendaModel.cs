using EstoqueProdutos.Models;
using System.ComponentModel.DataAnnotations;

public class ProdutoVendaModel
{
    public int Id { get; set; }

    public int VendaId { get; set; }

    public VendaModel Venda { get; set; }

    [Required(ErrorMessage = "O Produto é obrigatório.")]
    public int ProdutoId { get; set; }

    public ProdutoModel Produto { get; set; }

    public string NomeProduto { get; set; }

    [Required(ErrorMessage = "O campo Quantidade é obrigatório.")]
    public int Quantidade { get; set; }

    public decimal ValorProduto { get; set; }
}