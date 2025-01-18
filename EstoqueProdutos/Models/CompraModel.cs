using EstoqueProdutos.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EstoqueProdutos.Models
{
    public class CompraModel
    {
        [Column("ID_COMPRA")]
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo é obrigatório")]
        [Column("ID_FORNECEDOR")]
        public int FornecedorId { get; set; }

        public FornecedorModel Fornecedor { get; set; }

        [Required(ErrorMessage = "O campo é obrigatório")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        [Column("VALOR_DA_COMPRA")]
        public decimal? Valor { get; set; }

        [Column("FORMA_PAGAMAENTO")]
        public FormaDePagamentoEnum FormaDePagamento { get; set; }

        [Column("PARCELADO")]
        public bool Parcelado { get; set; }

        [Column("QTDE_PARCELA")]
        public int QuantidadeDeParcela { get; set; }

        [Column("QTDE_PRODUTO")]
        public int QuantidadeProduto { get; set; }

        [Column("DT_INC")]
        public DateTime DtInc { get; set; } = DateTime.Now;

        [Column("DT_ALT")]
        public DateTime DtAlt { get; set;  } = DateTime.Now;

        [Column("ID_USUARIO")]
        public int UsuarioId { get; set; }

        [NotMapped]
        public UsuarioModel Usuario { get; set; }

        [NotMapped]
        public List<int> ProdutoCompraId { get; set; } = new List<int>();
        [NotMapped]
        public List<ProdutoCompraModel> ProdutoCompra { get; set; } = new List<ProdutoCompraModel>();

    }
}
