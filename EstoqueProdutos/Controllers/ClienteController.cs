using EstoqueProdutos.Data;
using EstoqueProdutos.Enums;
using EstoqueProdutos.Models;
using EstoqueProdutos.Repositorio;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace EstoqueProdutos.Controllers
{
    public class ClienteController : Controller
    {
        readonly private IClienteRepositorio _clienteRepositorio;

        public ClienteController(IClienteRepositorio clienteRepositorio)
        {
            _clienteRepositorio = clienteRepositorio;
        }

        public IActionResult Index()
        {
            var Clientes = _clienteRepositorio.ListarTodos();
            return View(Clientes);
        }

        [HttpGet]
        public IActionResult Criar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Criar(ClienteModel cliente)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _clienteRepositorio.Adicionar(cliente);
                    TempData["MensagemSucesso"] = "Cliente cadastrado com sucesso!";
                    return RedirectToAction("Index");
                }
                return View();
            }
            catch (Exception err)
            {
                TempData["MensagemErro"] = $"Ops, não conseguimos criar seu novo cliente, detalhe do erro: {err.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public IActionResult Editar(int id)
        {
            ClienteModel cliente = _clienteRepositorio.ListarPorId(id);
            return View(cliente);
        }

        [HttpPost]
        public IActionResult Editar(ClienteModel cliente)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _clienteRepositorio.Alterar(cliente);
                    TempData["MensagemSucesso"] = "Cliente editado com sucesso!";
                    return RedirectToAction("Index");
                }

                return View();
            }
            catch (Exception err)
            {
                TempData["MensagemErro"] = $"Ops, não conseguimos editar seu cliente, detalhe do erro: {err.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult Apagar(ClienteModel cliente)
        {
            _clienteRepositorio.Deletar(cliente);
            return RedirectToAction("Index");
        }
    }
}