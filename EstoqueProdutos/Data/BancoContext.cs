using EstoqueProdutos.Models;
using Microsoft.EntityFrameworkCore;

namespace EstoqueProdutos.Data
{
    public class BancoContext : DbContext
    {
        public BancoContext(DbContextOptions<BancoContext> options) : base(options)
        {
            
        }

        public DbSet<CompraModel> Compras { get; set; }
        public DbSet<ClienteModel> Clientes { get; set; }
        public DbSet<ConfiguracaoModal> Configuracao {  get; set; }
        public DbSet<FornecedorModel> Fornecedores { get; set; }
        public DbSet<ProdutoModel> Produtos { get; set; }
        public DbSet<ProdutoCompraModel> ProdutoCompra { get; set; }
        public DbSet<ProdutoVendaModel> ProdutoVenda { get; set; }
        public DbSet<UsuarioModel> Usuarios { get; set; }
        public DbSet<VendaModel> Vendas { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProdutoCompraModel>()
                .HasOne(pv => pv.Compra)
                .WithMany(v => v.ProdutoCompra)
                .HasForeignKey(pv => pv.CompraId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProdutoCompraModel>()
                .HasOne(pv => pv.Produto)
                .WithMany()
                .HasForeignKey(pv => pv.ProdutoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProdutoVendaModel>()
                .HasOne(pv => pv.Venda)
                .WithMany(v => v.ProdutoVenda)
                .HasForeignKey(pv => pv.VendaId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProdutoVendaModel>()
                .HasOne(pv => pv.Produto)
                .WithMany()
                .HasForeignKey(pv => pv.ProdutoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
