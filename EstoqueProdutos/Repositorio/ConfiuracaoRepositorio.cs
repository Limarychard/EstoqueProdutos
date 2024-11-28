using EstoqueProdutos.Data;
using EstoqueProdutos.Models;
using Microsoft.EntityFrameworkCore;

namespace EstoqueProdutos.Repositorio
{
    public class ConfiguracaoRepositorio : IConfiguracaoRepositorio
    {
        private readonly BancoContext _bancoContext;
        public ConfiguracaoRepositorio(BancoContext bancoContext)
        {
            _bancoContext = bancoContext;
        }

        public ConfiguracaoModal BuscarConfiguracaoPorUsuarioId(int usuarioId)
        {
            return _bancoContext.Configuracao.FirstOrDefault(c => c.UsuarioId == usuarioId);
        }

        public ConfiguracaoModal GerarConfiguracaoPadrao(ConfiguracaoModal configuracao)
        {
            _bancoContext.Configuracao.Add(configuracao);
            _bancoContext.SaveChanges();
            return configuracao;
        }

        public ConfiguracaoModal AtualizarConfiguracao(ConfiguracaoModal configuracao)
        {
            _bancoContext.Update(configuracao);
            _bancoContext.SaveChanges();
            return configuracao;
        }
    }
}
