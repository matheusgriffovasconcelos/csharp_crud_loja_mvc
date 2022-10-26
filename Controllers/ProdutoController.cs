using Aula5.Data;
using Aula5.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Aula5.Controllers;

public class ProdutoController : Controller
{
    private readonly AppDbContext _db;

    public ProdutoController(AppDbContext db)
    {
        _db = db;
    }

    public IActionResult Index()
    {
        var produtos = _db.Produtos
            .Include(p => p.Categoria)
            .AsNoTracking()
            .OrderBy(p => p.Nome)
            .ToList();
        return View(produtos);
    }

    private void CarregarCategorias(int? idCategoria = null)
    {
        var categorias = _db.Categorias.OrderBy(c => c.Nome).ToList();
        var categoriasSelectList = new SelectList(
            categorias, "Id", "Nome", idCategoria);
        ViewBag.Categorias = categoriasSelectList;
    }

    [HttpGet]
    public IActionResult Cadastrar()
    {
        CarregarCategorias();
        var produto = new ProdutoModel();
        return View(produto);
    }

    [HttpPost]
    public IActionResult Cadastrar(ProdutoModel produto)
    {
        if (!ModelState.IsValid)
        {
            CarregarCategorias(produto.IdCategoria);
            return View(produto);
        }
        _db.Produtos.Add(produto);
        _db.SaveChanges();
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Alterar(int id)
    {
        var produto = _db.Produtos.Find(id);
        if (produto is null)
        {
            return RedirectToAction("Index");
        }
        CarregarCategorias(produto.IdCategoria);
        return View(produto);
    }

    [HttpPost]
    public IActionResult Alterar(int id, ProdutoModel produto)
    {
        var produtoOriginal = _db.Produtos.Find(id);
        if (produtoOriginal is null)
        {
            return RedirectToAction("Index");
        }
        if (!ModelState.IsValid)
        {
            CarregarCategorias(produto.IdCategoria);
            return View(produto);
        }
        produtoOriginal.Nome = produto.Nome;
        produtoOriginal.Estoque = produto.Estoque;
        produtoOriginal.Preco = produto.Preco;
        produtoOriginal.IdCategoria = produto.IdCategoria;
        _db.SaveChanges();
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Excluir(int id)
    {
        var produto = _db.Produtos.Find(id);
        if (produto is null)
        {
            return RedirectToAction("Index");
        }
        return View(produto);
    }

    [HttpPost]
    public IActionResult ProcessarExclusao(int id)
    {
        var produtoOriginal = _db.Produtos.Find(id);
        if (produtoOriginal is null)
        {
            return RedirectToAction("Index");
        }
        _db.Produtos.Remove(produtoOriginal);
        _db.SaveChanges();
        return RedirectToAction("Index");
    }
}