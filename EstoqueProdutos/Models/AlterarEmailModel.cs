using System.ComponentModel.DataAnnotations;

namespace EstoqueProdutos.Models
{
    public class AlterarEmailModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Digite o email atual do usuário")]
        public string EmailAtual { get; set; }

        [Required(ErrorMessage = "Digite o novo email do usuário")]
        public string NovoEmail { get; set; }

        [Required(ErrorMessage = "Confirme o novo email do usuário")]
        [Compare("NovoEmail", ErrorMessage = "O email não confere com o novo email")]
        public string ConfirmarNovoEmail { get; set; }
 
    }
}
