using EstoqueProdutos.Helper;
using EstoqueProdutos.Models;
using EstoqueProdutos.Repositorio;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

public class LoginController : Controller
{
    private IUsuarioRepositorio _usuarioRepositorio;
    private ISessao _sessao;
    private readonly IEmail _email;

    public LoginController(
        IUsuarioRepositorio usuarioRepositorio,
        ISessao sessao,
        IEmail email
    )
    {
        _usuarioRepositorio = usuarioRepositorio;
        _sessao = sessao;
        _email = email;
    }

    [HttpGet]
    public IActionResult Index()
    {
        // Se o usuário estiver logado, redirecionar para a home.

        if (_sessao.BuscarSessaoDoUsuario() != null) return RedirectToAction("Index", "Home");

        return View();
    }

    public IActionResult RedefinirSenha()
    {
        return View();
    }

    public IActionResult Sair()
    {
        _sessao.RemoverSessaoUsuaroio();

        return RedirectToAction("Index", "Login");
    }

    [HttpPost]
    public IActionResult Entrar(string Login, string Senha)
    {
        try
        {
            UsuarioModel usuario = _usuarioRepositorio.BuscarPorLogin(Login);


            if (ModelState.IsValid)
            {
                if (usuario != null && usuario.SenhaValida(Senha))
                {
                    _sessao.CriarSessaoUsuaroio(usuario);
                    return RedirectToAction("Index", "Home");
                }

                TempData["MensagemErro"] = $"Usuário e/ou senha inválido(s). Por favor, tente novamente.";
            }
            return View("Index");
        }
        catch (Exception err)
        {
            TempData["MensagemErro"] = $"Ops, ocorreu um erro: {err.Message}";
            return RedirectToAction("Index");
        }
    }

    [HttpPost]
    public IActionResult EnviarLinkParaRedefinirSenha(RedefinirSenhaModel redefinirSenhaModel)
    {
        try
        {
            UsuarioModel usuario = _usuarioRepositorio.BuscarPorEmailELogin(redefinirSenhaModel.Login, redefinirSenhaModel.Email);


            if (ModelState.IsValid)
            {
                if (usuario != null)
                {
                    string novaSenha = usuario.GerarNovaSenha();
                    string mensagem = $"Sua nova senha é: {novaSenha}";

                    bool emailEnviado = _email.Enviar(usuario.Email, "Sistema de contatos - Nova senha", mensagem);

                    if (emailEnviado)
                    {
                        _usuarioRepositorio.Alterar(usuario);
                        TempData["MensagemSucesso"] = $"Enviamos uma nova senha para seu e-mail.";
                    }
                    else
                    {
                        TempData["MensagemErro"] = $"Não conseguimos enviar o e-mail. Por favor, tente novamente!";
                    }

                    return RedirectToAction("Index", "Login");
                }

                TempData["MensagemErro"] = $"Não conseguimos redefinir sua senha. Por favor, verifique os dados informados!";
            }
            return View("Index");
        }
        catch (Exception err)
        {
            TempData["MensagemErro"] = $"Ops, não conseguimos redefinir sua senha: {err.Message}";
            return RedirectToAction("Index");
        }
    }

}