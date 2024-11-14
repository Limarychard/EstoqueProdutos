using EstoqueProdutos.Data;
using EstoqueProdutos.Models;
using Microsoft.EntityFrameworkCore;

namespace EstoqueProdutos.Repositorio
{
    public class ClienteRepositorio : IClienteRepositorio
    {
        private readonly BancoContext _bancoContext;
        public ClienteRepositorio(BancoContext bancoContext)
        {
            _bancoContext = bancoContext;
        }

        public ClienteModel ListarPorId(int id)
        {
            return _bancoContext.Clientes.FirstOrDefault(x => x.Id == id);
        }

        public List<ClienteModel> ListarPorUsuarioId(int usuarioId)
        {
            return _bancoContext.Clientes
                .Where(v => v.UsuarioId == usuarioId)
                .ToList();
        }

        public List<ClienteModel> ListarTodos()
        {
            return _bancoContext.Clientes.ToList();
        }

        public ClienteModel Adicionar(ClienteModel cliente)
        {
            _bancoContext.Clientes.Add(cliente);
            _bancoContext.SaveChanges();
            return cliente;
        }

        public ClienteModel Alterar(ClienteModel cliente)
        {
            _bancoContext.Update(cliente);
            _bancoContext.SaveChanges();
            return cliente;
        }

        public ClienteModel Deletar(ClienteModel cliente)
        {
            _bancoContext.Remove(cliente);
            _bancoContext.SaveChanges();
            return cliente;
        }

    }
}
