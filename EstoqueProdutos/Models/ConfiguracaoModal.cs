namespace EstoqueProdutos.Models
{
    public class ConfiguracaoModal
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int TemaId { get; set; }
        public int FonteId { get; set; } = 1;
        public int PesquisarProduto { get; set; }

        public UsuarioModel Usuario { get; set; }
    }
}
