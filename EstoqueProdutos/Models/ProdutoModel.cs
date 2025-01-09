using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EstoqueProdutos.Models
{
    public class ProdutoModel
    {
        [Column("ID_PRODUTO")]
        public int Id { get; set; }

        [Column("NOME")]
        [Required(ErrorMessage = "Digite o nome do produto")]
        public string Nome { get; set; }

        [Column("VALOR_DE_VENDA")]
        [Required(ErrorMessage = "Digite o valor da venda do produto")]
        public decimal ValorDeVenda { get; set; }

        [Column("VALOR_DE_CUSTO")]
        [Required(ErrorMessage = "Digite o valor pago no produto")]
        public decimal ValorDeCusto { get; set; }

        [Column("CODIGO")]
        public string? Codigo { get; set; }

        [Column("DESCRICAO")]
        [Required(ErrorMessage = "Digite a descrição do produto")]
        public string Descricao { get; set; }

        [Column("FOTO")]
        public byte[]? Foto { get; set; }

        [Column("QUANTIDADE")]
        [Required(ErrorMessage = "Digite a quantidade do produto")]
        public int Quantidade { get; set; }

        [Column("ID_USUARIO")]
        public int UsuarioId { get; set; }

        public UsuarioModel Usuario { get; set; }
    }
}
