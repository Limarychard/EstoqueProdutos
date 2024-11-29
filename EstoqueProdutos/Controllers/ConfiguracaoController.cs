using EstoqueProdutos.Filters;
using EstoqueProdutos.Helper;
using EstoqueProdutos.Models;
using EstoqueProdutos.Repositorio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace EstoqueProdutos.Controllers
{
    [PaginaParaUsuarioLogado]
    public class ConfiguracaoController : Controller
    {

        private IUsuarioRepositorio _usuarioRepositorio;
        private IConfiguracaoRepositorio _configuracaoRepositorio;
        private ISessao _sessao;

        public ConfiguracaoController(
            IUsuarioRepositorio usuarioRepositorio,
            IConfiguracaoRepositorio configuracaoRepositorio,
            ISessao sessao)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _configuracaoRepositorio = configuracaoRepositorio;
            _sessao = sessao;
        }

        public IActionResult Index(string aba = "conta")
        {
            ViewData["AbaAtiva"] = aba;
            UsuarioModel usuarioLogado = _sessao.BuscarSessaoDoUsuario();
            var configuracao = _configuracaoRepositorio.BuscarConfiguracaoPorUsuarioId(usuarioLogado.Id);
            var viewModel = new ConfiguracaoViewModel();

            if (configuracao == null)
            {
                viewModel = new ConfiguracaoViewModel
                {
                    AlterarSenha = new AlterarSenhaModel(),
                    AlterarEmail = new AlterarEmailModel(),
                    Configuracao = _configuracaoRepositorio.GerarConfiguracaoPadrao(new ConfiguracaoModal
                    {
                        FonteId = 0,
                        TemaId = 0,
                        UsuarioId = usuarioLogado.Id
                    })
                };
            } else
            {
                viewModel = new ConfiguracaoViewModel
                {
                    AlterarSenha = new AlterarSenhaModel(),
                    AlterarEmail = new AlterarEmailModel(),
                    Configuracao = new ConfiguracaoModal
                    {
                        Id = configuracao.Id,
                        FonteId = configuracao.FonteId,
                        TemaId = configuracao.TemaId,
                        Usuario = configuracao.Usuario,
                        UsuarioId = configuracao.UsuarioId
                    }
                };


            }

            return View(viewModel);

        }

        [HttpPost]
        public IActionResult AlterarSenha(ConfiguracaoViewModel viewModel)
        {
            try
            {
                UsuarioModel usuarioLogado = _sessao.BuscarSessaoDoUsuario();

                viewModel.AlterarSenha.Id = usuarioLogado.Id;

                ModelState.Remove("AlterarEmail");

                if (ModelState.IsValid)
                {
                    _usuarioRepositorio.AlterarSenha(viewModel.AlterarSenha);
                    TempData["MensagemSucesso"] = "Senha alterada com sucesso!";
                    _sessao.RemoverSessaoUsuaroio();
                    return RedirectToAction("Index", "Login");
                }

                return RedirectToAction("Index", viewModel);
            }
            catch (Exception err)
            {
                TempData["MensagemErro"] = $"Ops, não conseguimos alterar sua senha, detalhe do erro: {err.Message}";
                return RedirectToAction("Index", viewModel);
            }
        }

        [HttpPost]
        public IActionResult AlterarEmail(ConfiguracaoViewModel viewModel)
        {
            try
            {
                UsuarioModel usuarioLogado = _sessao.BuscarSessaoDoUsuario();

                viewModel.AlterarEmail.Id = usuarioLogado.Id;

                ModelState.Remove("AlterarSenha");

                if (ModelState.IsValid)
                {
                    _usuarioRepositorio.AlterarEmail(viewModel.AlterarEmail);
                    TempData["MensagemSucesso"] = "Email alterado com sucesso!";
                    return RedirectToAction("Index");
                }

                return RedirectToAction("Index");

            }
            catch (Exception err)
            {
                TempData["MensagemErro"] = $"Ops, não conseguimos alterar seu email, detalhe do erro: {err.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult SalvarPreferencias(ConfiguracaoModal configuracao)
        {

            try
            {
                UsuarioModel usuarioLogado = _sessao.BuscarSessaoDoUsuario();
                configuracao.UsuarioId = usuarioLogado.Id;

                ModelState.Remove("Usuario");

                if (ModelState.IsValid)
                {
                    var configuracaoExistente = _configuracaoRepositorio.BuscarConfiguracaoPorUsuarioId(usuarioLogado.Id);


                    if (configuracaoExistente != null)
                    {
                        configuracaoExistente.TemaId = configuracao.TemaId;
                        configuracaoExistente.FonteId = configuracao.FonteId;

                        _configuracaoRepositorio.AtualizarConfiguracao(configuracaoExistente);
                    }

                    TempData["MensagemSucesso"] = "Preferência salva com sucesso!";

                } else
                {
                    TempData["MensagemErro"] = $"Ops, não conseguimos salvar suas preferências, por favor entre em contato com o administrador do sistema!";
                }
                
                return RedirectToAction("Index", configuracao);
            }
            catch(Exception err) 
            {
                TempData["MensagemErro"] = $"Ops, não conseguimos salvar suas preferências, detalhe do erro: {err.Message}";
                return RedirectToAction("Index");
            }

        }

        [HttpGet]
        public JsonResult ObterPreferencias()
        {
            try
            {
                UsuarioModel usuarioLogado = _sessao.BuscarSessaoDoUsuario();

                if (usuarioLogado == null)
                {
                    return Json(new { erro = "Usuário não autenticado." });
                }
                var configuracao = _configuracaoRepositorio.BuscarConfiguracaoPorUsuarioId(usuarioLogado.Id);

                if (configuracao != null)
                {
                    var tema = configuracao.TemaId == 1 ? "dark" : "light";
                    return Json(new { tema });
                }

                return Json(new { tema = "light" });
            }
            catch (Exception err)
            {
                return Json(new { erro = $"Erro ao obter preferências: {err.Message}" });
            }
        }
    }
}
