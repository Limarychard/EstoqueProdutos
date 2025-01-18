using EstoqueProdutos.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class ProdutoCompraModel
{
    [Column("ID_PRODUTO_COMPRA")]
    public int Id { get; set; }

    [Column("ID_COMPRA")]
    public int CompraId { get; set; }

    public CompraModel Compra { get; set; }

    [Required(ErrorMessage = "O Produto é obrigatório.")]
    [Column("ID_PRODUTO")]
    public int ProdutoId { get; set; }

    public ProdutoModel Produto { get; set; }

    [Column("NOME_PRODUTO")]
    public string NomeProduto { get; set; }

    [Required(ErrorMessage = "O campo Quantidade é obrigatório.")]
    [Column("QTDE_PRODUTO")]
    public int Quantidade { get; set; }

    [Column("VALOR_PRODUTO")]
    public decimal ValorProduto { get; set; }
}