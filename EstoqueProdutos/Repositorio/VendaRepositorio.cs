using EstoqueProdutos.Data;
using EstoqueProdutos.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EstoqueProdutos.Repositorio
{
    public class VendaRepositorio : IVendaRepositorio
    {
        private readonly BancoContext _bancoContext;
        public VendaRepositorio(BancoContext bancoContext)
        {
            _bancoContext = bancoContext;
        }

        public VendaModel ListarPorId(int id)
        {
            // Inclui a coleção de produtos da venda
            return _bancoContext.Vendas
                .Include(v => v.ProdutoVenda) // Inclui os produtos relacionados à venda
                .FirstOrDefault(x => x.Id == id);
        }

        public List<VendaModel> ListarTodos()
        {
            return _bancoContext.Vendas
                .Include(v => v.Cliente)
                .Include(v => v.ProdutoVenda)
                    .ThenInclude(pv => pv.Produto)
                .ToList();
        }

        public List<VendaModel> ListarPorUsuarioId(int usuarioId)
        {
            return _bancoContext.Vendas
                .Include(v => v.Cliente)
                .Include(v => v.ProdutoVenda)
                .ThenInclude(pv => pv.Produto)
                .Where(v => v.UsuarioId == usuarioId)
                .ToList();
        }

        public VendaModel Adicionar(VendaModel venda, List<ProdutoVendaModel> produtos)
        {
            venda.ProdutoVenda = produtos;
            _bancoContext.Vendas.Add(venda);
            _bancoContext.SaveChanges();
            return venda;
        }

        public VendaModel Alterar(VendaModel venda)
        {
            VendaModel vendaDB = ListarPorId(venda.Id);

            if (vendaDB == null) throw new Exception("Houve um erro na atualização da venda!");

            
            vendaDB.ClienteId = venda.ClienteId;
            vendaDB.Valor = venda.Valor;
            vendaDB.FormaDePagamento = venda.FormaDePagamento;
            vendaDB.Parcelado = venda.Parcelado;
            vendaDB.QuantidadeDeParcela = venda.QuantidadeDeParcela;
            vendaDB.DtAlt = DateTime.Now;

            _bancoContext.Vendas.Update(vendaDB);
            _bancoContext.SaveChanges();

            return vendaDB;
        }

        public VendaModel Deletar(VendaModel venda)
        {
            _bancoContext.Remove(venda);
            _bancoContext.SaveChanges();
            return venda;
        }

        public ProdutoVendaModel ListarProdutoVendaPorId(int id)
        {
            return _bancoContext.ProdutoVenda.FirstOrDefault(x => x.Id == id);
        }

        public ProdutoVendaModel AlterarProdutoVenda(ProdutoVendaModel produtoVenda)
        {
            ProdutoVendaModel produtoVendaDB = ListarProdutoVendaPorId(produtoVenda.Id);

            if (produtoVendaDB == null) throw new Exception("Houve um erro na atualização da venda!");


            produtoVendaDB.VendaId = produtoVenda.VendaId;
            produtoVendaDB.ProdutoId = produtoVenda.ProdutoId;
            produtoVendaDB.NomeProduto = produtoVenda.NomeProduto;
            produtoVendaDB.ValorProduto = produtoVenda.ValorProduto;

            _bancoContext.ProdutoVenda.Update(produtoVenda);
            _bancoContext.SaveChanges();

            return produtoVendaDB;
        }


        public ProdutoVendaModel DeletarProduto(ProdutoVendaModel produtoVendaModel)
        {
            _bancoContext.Remove(produtoVendaModel);
            _bancoContext.SaveChanges();
            return produtoVendaModel;
        }
    }
}
