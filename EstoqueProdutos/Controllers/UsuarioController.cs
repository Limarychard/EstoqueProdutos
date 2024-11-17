using EstoqueProdutos.Data;
using EstoqueProdutos.Enums;
using EstoqueProdutos.Filters;
using EstoqueProdutos.Models;
using EstoqueProdutos.Repositorio;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace EstoqueProdutos.Controllers
{
    [PaginaRestritaSomenteAdmin]
    public class UsuarioController : Controller
    {
        readonly private IUsuarioRepositorio _usuarioRepositorio;

        public UsuarioController(IUsuarioRepositorio usuarioRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
        }

        public IActionResult Index()
        {
            var Clientes = _usuarioRepositorio.ListarTodos();
            return View(Clientes);
        }

        public IActionResult ObterImagem(int id)
        {
            var usuario = _usuarioRepositorio.ObterPorId(id);
            if (usuario != null && usuario.Logo != null)
            {
                return File(usuario.Logo, "image/jpeg");
            }

            return NotFound();
        }

        [HttpGet]
        public IActionResult Criar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Criar(UsuarioModel usuario, IFormFile Logo)
        {
            try
            {
                if (Logo != null && Logo.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        Logo.CopyTo(memoryStream);
                        usuario.Logo = memoryStream.ToArray();
                    }
                }

                ModelState.Remove("Logo");

                if (ModelState.IsValid)
                {
                    usuario.SetSenhaHash();

                    _usuarioRepositorio.Adicionar(usuario);
                    TempData["MensagemSucesso"] = "Usuário cadastrado com sucesso!";
                    return RedirectToAction("Index");
                }
                return View();
            }
            catch (Exception err)
            {
                TempData["MensagemErro"] = $"Ops, não conseguimos cadastrar seu novo usuário, detalhe do erro: {err.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public IActionResult Editar(int id)
        {
            UsuarioModel usuario = _usuarioRepositorio.ListarPorId(id);
            return View(usuario);
        }

        [HttpPost]
        public IActionResult Editar(UsuarioSemSenhaModel usuarioSemSenha, IFormFile Logo)
        {
            try
            {
                if (Logo != null && Logo.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        Logo.CopyTo(memoryStream);
                        usuarioSemSenha.Logo = memoryStream.ToArray();
                    }
                }
                else
                {
                    var usuarioExistente = _usuarioRepositorio.ObterPorId(usuarioSemSenha.Id);
                    if (usuarioExistente != null)
                    {
                        usuarioSemSenha.Logo = usuarioExistente.Logo;
                    }
                }

                ModelState.Remove("Logo");

                UsuarioModel usuario = null;

                if (ModelState.IsValid)
                {
                    usuario = new UsuarioModel()
                    {
                        Id = usuarioSemSenha.Id,
                        Nome = usuarioSemSenha.Nome,
                        Login = usuarioSemSenha.Login,
                        Email = usuarioSemSenha.Email,
                        Perfil = usuarioSemSenha.Perfil,
                        Logo = usuarioSemSenha.Logo
                    };

                    usuario = _usuarioRepositorio.Alterar(usuario);
                    TempData["MensagemSucesso"] = "Usuário editado com sucesso!";
                    return RedirectToAction("Index");
                }

                return View(usuario);
            }
            catch (Exception err)
            {
                Console.WriteLine(err.InnerException?.Message);
                TempData["MensagemErro"] = $"Ops, não conseguimos editar seu usuário, detalhe do erro: {err.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult Apagar(UsuarioModel usuario)
        {
            _usuarioRepositorio.Deletar(usuario);
            return RedirectToAction("Index");
        }
    }
}