﻿using EstoqueProdutos.Data;
using EstoqueProdutos.Enums;
using EstoqueProdutos.Models;
using EstoqueProdutos.Repositorio;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace EstoqueProdutos.Controllers
{
    public class ProdutoController : Controller
    {
        readonly private IProdutoRepositorio _produtoRepositorio;

        public ProdutoController(IProdutoRepositorio produtoRepositorio)
        {
            _produtoRepositorio = produtoRepositorio;
        }

        public IActionResult Index()
        {
            var Produtos = _produtoRepositorio.ListarTodos();
            return View(Produtos);
        }

        public IActionResult ObterImagem(int id)
        {
            var produto = _produtoRepositorio.ObterPorId(id);
            if (produto != null && produto.Foto != null)
            {
                return File(produto.Foto, "image/jpeg");
            }

            return NotFound();
        }

        [HttpGet]
        public IActionResult Criar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Criar(ProdutoModel produto, IFormFile Foto)
        {
            try
            {
                if (Foto != null && Foto.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        Foto.CopyTo(memoryStream);
                        produto.Foto = memoryStream.ToArray();
                    }
                }

                ModelState.Remove("Foto");

                if (ModelState.IsValid)
                {
                    _produtoRepositorio.Adicionar(produto);
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
            ProdutoModel produto = _produtoRepositorio.ListarPorId(id);
            return View(produto);
        }

        [HttpPost]
        public IActionResult Editar(ProdutoModel produto, IFormFile Foto)
        {
            try
            {
                if (Foto != null && Foto.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        Foto.CopyTo(memoryStream);
                        produto.Foto = memoryStream.ToArray();
                    }
                }
                else
                {
                    var produtoExistente = _produtoRepositorio.ObterPorId(produto.Id);
                    if (produtoExistente != null)
                    {
                        produto.Foto = produtoExistente.Foto;
                    }
                }

                ModelState.Remove("Foto");

                if (ModelState.IsValid)
                {
                    _produtoRepositorio.Alterar(produto);
                    TempData["MensagemSucesso"] = "Produto editado com sucesso!";
                    return RedirectToAction("Index");
                }

                TempData["MensagemErro"] = "Dados inválidos. Por favor, corrija os erros e tente novamente.";
                return View(produto);
            }
            catch (Exception err)
            {
                TempData["MensagemErro"] = $"Erro: {err.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult Apagar(ProdutoModel produto)
        {
            _produtoRepositorio.Deletar(produto);
            return RedirectToAction("Index");
        }
    }
}