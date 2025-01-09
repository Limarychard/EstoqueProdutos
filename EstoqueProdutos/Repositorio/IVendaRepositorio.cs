using EstoqueProdutos.Models;

namespace EstoqueProdutos.Repositorio
{
    public interface IVendaRepositorio
    {
        VendaModel ListarPorId(int id);
        List<VendaModel> ListarTodos();
        List<VendaModel> ListarPorUsuarioId(int usuarioId);
        VendaModel Adicionar(VendaModel venda, List<ProdutoVendaModel> produtoVendas);
        VendaModel Alterar(VendaModel venda);
        VendaModel Deletar(VendaModel venda);

        ProdutoVendaModel ListarProdutoVendaPorId(int id);
        ProdutoVendaModel AdicionarProdutoVenda(ProdutoVendaModel produtoVenda);
        ProdutoVendaModel AlterarProdutoVenda(ProdutoVendaModel produtoVenda);
        ProdutoVendaModel DeletarProduto(ProdutoVendaModel produtoVendaModel);
    }
}
