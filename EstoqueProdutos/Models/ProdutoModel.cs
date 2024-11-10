namespace EstoqueProdutos.Models
{
    public class ProdutoModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public double Valor { get; set; }
        public string Descricao { get; set; }
        public byte[] Foto { get; set; }
        public int Quantidade { get; set; }
    }
}
