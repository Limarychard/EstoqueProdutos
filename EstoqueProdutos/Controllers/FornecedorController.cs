using EstoqueProdutos.Data;
using EstoqueProdutos.Enums;
using EstoqueProdutos.Filters;
using EstoqueProdutos.Helper;
using EstoqueProdutos.Models;
using EstoqueProdutos.Repositorio;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace EstoqueProdutos.Controllers
{
    [PaginaParaUsuarioLogado]
    public class FornecedorController : Controller
    {
        readonly private IFornecedorRepositorio _fornecedorRepositorio;
        readonly private ISessao _sessao;

        public FornecedorController(
            IFornecedorRepositorio fornecedorRepositorio,
            ISessao sessao
            )
        {
            _fornecedorRepositorio = fornecedorRepositorio;
            _sessao = sessao;
        }

        public IActionResult Index(string? Nome = null, long? Telefone = null, long? CPF = null, DateTime? DataUltimaCompraInicio = null, DateTime? DataUltimaCompraInicioFim = null)
        {
            var usuarioLogado = _sessao.BuscarSessaoDoUsuario();

            List<FornecedorModel> Fornecedores;
            if (usuarioLogado.Perfil == PerfilEnum.Admin)
            {
                Fornecedores = _fornecedorRepositorio.ListarTodos();
            }
            else
            {
                Fornecedores = _fornecedorRepositorio.ListarPorUsuarioId(usuarioLogado.Id);
            }

            if (!string.IsNullOrEmpty(Nome))
                Fornecedores = Fornecedores.Where(u => u.Nome.Contains(Nome, StringComparison.OrdinalIgnoreCase)).ToList();

            if (Telefone != 0)
            {
                string telefoneFiltro = Telefone.ToString();
                Fornecedores = Fornecedores.Where(u => u.Telefone.ToString().Contains(telefoneFiltro)).ToList();
            }

            if (CPF != null)
            {
                string CpfFiltro = CPF.ToString();
                Fornecedores = Fornecedores.Where(u => u.Telefone.ToString().Contains(CpfFiltro)).ToList();
            }

            if (DataUltimaCompraInicio.HasValue)
                Fornecedores = Fornecedores.Where(u => u.DtUltCompra >= DataUltimaCompraInicio.Value).ToList();

            if (DataUltimaCompraInicioFim.HasValue)
                Fornecedores = Fornecedores.Where(u => u.DtUltCompra <= DataUltimaCompraInicioFim.Value).ToList();

            ViewData["Nome"] = Nome;
            ViewData["Telefone"] = Telefone;
            ViewData["CPF"] = CPF;
            ViewData["DataUltimaCompraInicio"] = DataUltimaCompraInicio?.ToString("yyyy-MM-dd");
            ViewData["DataUltimaCompraInicioFim"] = DataUltimaCompraInicioFim?.ToString("yyyy-MM-dd");

            return View(Fornecedores);
        }

        [HttpGet]
        public IActionResult Criar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Criar(FornecedorModel fornecedor)
        {
            try
            {
                ModelState.Remove("Usuario");

                if (ModelState.IsValid)
                {

                    var usuarioLogado = _sessao.BuscarSessaoDoUsuario();
                    fornecedor.UsuarioId = usuarioLogado.Id;

                    _fornecedorRepositorio.Adicionar(fornecedor);
                    TempData["MensagemSucesso"] = "Fornecedor cadastrado com sucesso!";
                    return RedirectToAction("Index");
                }
                return View();
            }
            catch (Exception err)
            {
                TempData["MensagemErro"] = $"Ops, não conseguimos criar seu novo fornecedor, detalhe do erro: {err.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public IActionResult Editar(int id)
        {
            FornecedorModel fornecedor = _fornecedorRepositorio.ListarPorId(id);
            return View(fornecedor);
        }

        [HttpPost]
        public IActionResult Editar(FornecedorModel fornecedor)
        {
            try
            {
                ModelState.Remove("Usuario");

                if (ModelState.IsValid)
                {
                    var usuarioLogado = _sessao.BuscarSessaoDoUsuario();
                    fornecedor.UsuarioId = usuarioLogado.Id;

                    _fornecedorRepositorio.Alterar(fornecedor);
                    TempData["MensagemSucesso"] = "Fornecedor editado com sucesso!";
                    return RedirectToAction("Index");
                }

                return View();
            }
            catch (Exception err)
            {
                TempData["MensagemErro"] = $"Ops, não conseguimos editar seu fornecedor, detalhe do erro: {err.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult Apagar(FornecedorModel fornecedor)
        {
            _fornecedorRepositorio.Deletar(fornecedor);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Enderecos(int id)
        {
            FornecedorModel fornecedor = _fornecedorRepositorio.ListarPorId(id);
            return View(fornecedor);
        }
    }
}