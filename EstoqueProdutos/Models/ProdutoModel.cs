using System.ComponentModel.DataAnnotations;

namespace EstoqueProdutos.Models
{
    public class ProdutoModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Digite o nome do produto")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Digite o valor do produto")]
        public double Valor { get; set; }

        [Required(ErrorMessage = "Digite a descrição do produto")]
        public string Descricao { get; set; }

        public byte[]? Foto { get; set; }

        [Required(ErrorMessage = "Digite a quantidade do produto")]
        public int Quantidade { get; set; }

        public int UsuarioId { get; set; }
        public UsuarioModel Usuario { get; set; }
    }
}
