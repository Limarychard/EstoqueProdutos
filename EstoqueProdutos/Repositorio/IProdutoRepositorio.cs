using EstoqueProdutos.Models;

namespace EstoqueProdutos.Repositorio
{
    public interface IProdutoRepositorio
    {
        ProdutoModel ListarPorId(int id);
        List<ProdutoModel> ListarTodos();
        ProdutoModel ObterPorId(int id);
        ProdutoModel Adicionar(ProdutoModel produto);
        ProdutoModel Alterar(ProdutoModel produto);
        ProdutoModel Deletar(ProdutoModel produto);
    }
}
