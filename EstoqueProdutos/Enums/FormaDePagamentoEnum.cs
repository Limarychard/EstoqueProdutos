using System.ComponentModel.DataAnnotations;

namespace EstoqueProdutos.Enums
{
    public enum FormaDePagamentoEnum
    {
        [Display(Name = "Cartão de Crédito")]
        CartaoDeCredito = 0,

        [Display(Name = "Dinheiro")]
        Dinheiro = 1,

        [Display(Name = "Pix")]
        Pix = 2,

        [Display(Name = "Boleto")]
        Boleto = 3,
    }
}