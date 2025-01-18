using EstoqueProdutos.Data;
using EstoqueProdutos.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EstoqueProdutos.Repositorio
{
    public class CompraRepositorio : ICompraRepositorio
    {
        private readonly BancoContext _bancoContext;
        public CompraRepositorio(BancoContext bancoContext)
        {
            _bancoContext = bancoContext;
        }

        public CompraModel ListarPorId(int id)
        {
            return _bancoContext.Compras
                .Include(v => v.ProdutoCompra)
                .FirstOrDefault(x => x.Id == id);
        }

        public List<CompraModel> ListarTodos()
        {
            return _bancoContext.Compras
                .Include(v => v.Fornecedor)
                .Include(v => v.ProdutoCompra)
                    .ThenInclude(pv => pv.Produto)
                .ToList();
        }

        public List<CompraModel> ListarPorUsuarioId(int usuarioId)
        {
            return _bancoContext.Compras
                .Include(v => v.Fornecedor)
                .Include(v => v.ProdutoCompra)
                .ThenInclude(pv => pv.Produto)
                .Where(v => v.UsuarioId == usuarioId)
                .ToList();
        }

        public CompraModel Adicionar(CompraModel compra, List<ProdutoCompraModel> produtos)
        {
            compra.ProdutoCompra = produtos;
            _bancoContext.Compras.Add(compra);
            _bancoContext.SaveChanges();
            return compra;
        }

        public CompraModel Alterar(CompraModel compra)
        {
            CompraModel compraDB = ListarPorId(compra.Id);

            if (compraDB == null) throw new Exception("Houve um erro na atualização da compra!");

            
            compraDB.FornecedorId = compra.FornecedorId;
            compraDB.Valor = compra.Valor;
            compraDB.FormaDePagamento = compra.FormaDePagamento;
            compraDB.Parcelado = compra.Parcelado;
            compraDB.QuantidadeDeParcela = compra.QuantidadeDeParcela;
            compraDB.DtAlt = DateTime.Now;

            _bancoContext.Compras.Update(compraDB);
            _bancoContext.SaveChanges();

            return compraDB;
        }

        public CompraModel Deletar(CompraModel compra)
        {
            _bancoContext.Remove(compra);
            _bancoContext.SaveChanges();
            return compra;
        }

        public ProdutoCompraModel ListarProdutoCompraPorId(int id)
        {
            return _bancoContext.ProdutoCompra.FirstOrDefault(x => x.Id == id);
        }

        public ProdutoCompraModel AdicionarProdutoCompra(ProdutoCompraModel produtoCompra)
        {
            _bancoContext.ProdutoCompra.Add(produtoCompra);
            _bancoContext.SaveChanges();
            return produtoCompra;
        }

        public ProdutoCompraModel AlterarProdutoCompra(ProdutoCompraModel produtoCompra)
        {
            ProdutoCompraModel produtoCompraDB = ListarProdutoCompraPorId(produtoCompra.Id);

            if (produtoCompraDB == null) throw new Exception("Houve um erro na atualização da compra!");


            produtoCompraDB.CompraId = produtoCompra.CompraId;
            produtoCompraDB.ProdutoId = produtoCompra.ProdutoId;
            produtoCompraDB.NomeProduto = produtoCompra.NomeProduto;
            produtoCompraDB.ValorProduto = produtoCompra.ValorProduto;

            _bancoContext.ProdutoCompra.Update(produtoCompra);
            _bancoContext.SaveChanges();

            return produtoCompraDB;
        }


        public ProdutoCompraModel DeletarProduto(ProdutoCompraModel produtoCompraModel)
        {
            _bancoContext.Remove(produtoCompraModel);
            _bancoContext.SaveChanges();
            return produtoCompraModel;
        }
    }
}
