using System.ComponentModel.DataAnnotations;

namespace EstoqueProdutos.Models
{
    public class ClienteModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo é obrigatório")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo é obrigatório")]
        public long Telefone { get; set; }
        public long? CPF { get; set; }

        public bool BomPagador { get; set; }
        public DateTime? DtUltCompra { get; set; }
        public DateTime DtInc { get; set; } = DateTime.Now;
        public DateTime DtAlt { get; set; } = DateTime.Now;

        public int UsuarioId { get; set; }
        public UsuarioModel Usuario { get; set; }
    }
}
