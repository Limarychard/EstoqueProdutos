using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EstoqueProdutos.Models
{
    public class ClienteModel
    {
        [Column("ID_CLIENTE")]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome do cliente é obrigatório")]
        [Column("NOME_CLIENTE")]
        public string Nome { get; set; }

        [ModelBinder(BinderType = typeof(LongBinder))]
        [Required(ErrorMessage = "O telefone do cliente é obrigatório")]
        [Column("TELEFONE")]
        public long Telefone { get; set; }

        [ModelBinder(BinderType = typeof(LongBinder))]
        [Column("CPF_CNPJ")]
        public long? CPF { get; set; }

        [ModelBinder(BinderType = typeof(LongBinder))]
        [Required(ErrorMessage = "O CEP do cliente é obrigatório")]
        [Column("CEP")]
        public int CEP { get; set; }

        [Required(ErrorMessage = "O Estado do cliente é obrigatório")]
        [Column("ESTADO")]
        public string Estado { get; set; }

        [Required(ErrorMessage = "A Cidade do cliente é obrigatório")]
        [Column("CIDADE")]
        public string Cidade { get; set; }

        [Required(ErrorMessage = "O Bairro do cliente é obrigatório")]
        [Column("BAIRRO")]
        public string Bairro { get; set; }

        [Required(ErrorMessage = "A Rua do cliente é obrigatório")]
        [Column("RUA")]
        public string Rua { get; set; }

        [Required(ErrorMessage = "O número do endereço do cliente é obrigatório")]
        [Column("NUMERO_ENDERECO")]
        public string Numero { get; set; }

        [Column("TEM_COMPLEMENTO")]
        public bool ExisteComplemento { get; set; }

        [Column("COMPLEMENTO")]
        public string? Complemento { get; set; }

        [Column("BOM_PAGADOR")]
        public bool BomPagador { get; set; }

        [Column("DT_ULT_COMPRA")]
        public DateTime? DtUltCompra { get; set; }

        [Column("DT_ALT")]
        public DateTime DtInc { get; set; } = DateTime.Now;

        [Column("DT_INC")]
        public DateTime DtAlt { get; set; } = DateTime.Now;

        [Column("ID_USUARIO")]
        public int UsuarioId { get; set; }

        public UsuarioModel? Usuario { get; set; }
    }
}
