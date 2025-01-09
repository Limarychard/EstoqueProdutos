using EstoqueProdutos.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EstoqueProdutos.Models
{
    public class VendaModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo é obrigatório")]
        public int ClienteId { get; set; }

        [Required(ErrorMessage = "O campo é obrigatório")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? Valor { get; set; }

        public ClienteModel Cliente { get; set; }

        public FormaDePagamentoEnum FormaDePagamento { get; set; }
        public bool Parcelado { get; set; }
        public int QuantidadeDeParcela { get; set; }
        public int QuantidadeProduto { get; set; }
        public DateTime DtInc { get; set; } = DateTime.Now;
        public DateTime DtAlt { get; set;  } = DateTime.Now;


        public int UsuarioId { get; set; }

        [NotMapped]
        public UsuarioModel Usuario { get; set; }

        [NotMapped]
        public List<int> ProdutoVendaId { get; set; } = new List<int>();
        [NotMapped]
        public List<ProdutoVendaModel> ProdutoVenda { get; set; } = new List<ProdutoVendaModel>();

    }
}
