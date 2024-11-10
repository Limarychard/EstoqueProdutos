using EstoqueProdutos.Models;

namespace EstoqueProdutos.Repositorio
{
    public interface IClienteRepositorio
    {
        ClienteModel ListarPorId(int id);
        List<ClienteModel> ListarTodos();
        ClienteModel Adicionar(ClienteModel cliente);
        ClienteModel Alterar(ClienteModel cliente);
        ClienteModel Deletar(ClienteModel cliente);
    }
}
