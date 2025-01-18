using EstoqueProdutos.Models;

namespace EstoqueProdutos.Repositorio
{
    public interface IFornecedorRepositorio
    {
        FornecedorModel ListarPorId(int id);
        List<FornecedorModel> ListarPorUsuarioId(int usuarioId);
        List<FornecedorModel> ListarTodos();
        FornecedorModel Adicionar(FornecedorModel fornecedor);
        FornecedorModel Alterar(FornecedorModel fornecedor);
        FornecedorModel Deletar(FornecedorModel fornecedor);
    }
}
