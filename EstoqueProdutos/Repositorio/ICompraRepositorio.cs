using EstoqueProdutos.Models;

namespace EstoqueProdutos.Repositorio
{
    public interface ICompraRepositorio
    {
        CompraModel ListarPorId(int id);
        List<CompraModel> ListarTodos();
        List<CompraModel> ListarPorUsuarioId(int usuarioId);
        CompraModel Adicionar(CompraModel compra, List<ProdutoCompraModel> produtoCompras);
        CompraModel Alterar(CompraModel compra);
        CompraModel Deletar(CompraModel compra);

        ProdutoCompraModel ListarProdutoCompraPorId(int id);
        ProdutoCompraModel AdicionarProdutoCompra(ProdutoCompraModel produtoCompra);
        ProdutoCompraModel AlterarProdutoCompra(ProdutoCompraModel produtoCompra);
        ProdutoCompraModel DeletarProduto(ProdutoCompraModel produtoCompraModel);
    }
}
