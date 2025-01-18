using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EstoqueProdutos.Models
{
    public class FornecedorModel
    {

        [Column("ID_FORNECEDOR")]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome do fornecedor é obrigatório")]
        [Column("NOME_FORNECEDOR")]
        public string Nome { get; set; }

        [ModelBinder(BinderType = typeof(LongBinder))]
        [Required(ErrorMessage = "O telefone do fornecedor é obrigatório")]
        [Column("TELEFONE")]
        public long Telefone { get; set; }

        [ModelBinder(BinderType = typeof(LongBinder))]
        [Column("CPF_CNPJ")]
        public long? CPF { get; set; }

        [Required(ErrorMessage = "O CEP do fornecedor é obrigatório")]
        [Column("CEP")]
        public string CEP { get; set; }

        [Required(ErrorMessage = "O Estado do fornecedor é obrigatório")]
        [Column("ESTADO")]
        public string Estado { get; set; }

        [Required(ErrorMessage = "A Cidade do fornecedor é obrigatório")]
        [Column("CIDADE")]
        public string Cidade { get; set; }

        [Required(ErrorMessage = "O Bairro do fornecedor é obrigatório")]
        [Column("BAIRRO")]
        public string Bairro { get; set; }

        [Required(ErrorMessage = "A Rua do fornecedor é obrigatório")]
        [Column("RUA")]
        public string Rua { get; set; }

        [Required(ErrorMessage = "O número do endereço do fornecedor é obrigatório")]
        [Column("NUMERO_ENDERECO")]
        public string Numero { get; set; }

        [Column("NAO_TEM_COMPLEMENTO")]
        public bool ExisteComplemento { get; set; }

        [Column("COMPLEMENTO")]
        public string? Complemento { get; set; }

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
