using EstoqueProdutos.Models;

namespace EstoqueProdutos.Repositorio
{
    public interface IConfiguracaoRepositorio
    {
        ConfiguracaoModal BuscarConfiguracaoPorUsuarioId(int usuarioId);
        ConfiguracaoModal GerarConfiguracaoPadrao(ConfiguracaoModal configuracao);
        ConfiguracaoModal AtualizarConfiguracao(ConfiguracaoModal configuracao);
    }
}
