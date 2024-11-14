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
            return _bancoContext.Vendas.FirstOrDefault(x => x.Id == id);
        }

        public List<VendaModel> ListarTodos()
        {
            return _bancoContext.Vendas.Include(v => v.Cliente).Include(v => v.Produto).ToList();
        }

        public List<VendaModel> ListarPorUsuarioId(int usuarioId)
        {
            return _bancoContext.Vendas
                .Include(v => v.Cliente)
                .Include(v => v.Produto)
                .Where(v => v.UsuarioId == usuarioId)
                .ToList();
        }

        public VendaModel Adicionar(VendaModel venda)
        {
            _bancoContext.Vendas.Add(venda);
            _bancoContext.SaveChanges();
            return venda;
        }

        public VendaModel Alterar(VendaModel venda)
        {
            _bancoContext.Update(venda);
            _bancoContext.SaveChanges();
            return venda;
        }

        public VendaModel Deletar(VendaModel venda)
        {
            _bancoContext.Remove(venda);
            _bancoContext.SaveChanges();
            return venda;
        }

    }
}
