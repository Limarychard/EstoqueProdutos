using EstoqueProdutos.Models;

namespace EstoqueProdutos.Repositorio
{
    public interface IVendaRepositorio
    {
        VendaModel ListarPorId(int id);
        List<VendaModel> ListarTodos();
        VendaModel Adicionar(VendaModel venda);
        VendaModel Alterar(VendaModel venda);
        VendaModel Deletar(VendaModel venda);
    }
}
