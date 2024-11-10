using EstoqueProdutos.Models;
using Microsoft.EntityFrameworkCore;

namespace EstoqueProdutos.Data
{
    public class BancoContext : DbContext
    {
        public BancoContext(DbContextOptions<BancoContext> options) : base(options)
        {
            
        }

        public DbSet<VendaModel> Vendas { get; set; }
        public DbSet<ClienteModel> Clientes { get; set; }
        public DbSet<ProdutoModel> Produtos { get; set; }
    }
}
