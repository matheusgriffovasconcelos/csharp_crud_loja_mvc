using Aula5.Data;
using Aula5.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Aula5.Controllers;

public class ItemPedidoController : Controller
{
    private readonly AppDbContext _db;

    public ItemPedidoController(AppDbContext db)
    {
        _db = db;
    }

    public IActionResult Index(int idPedido)
    {
        var pedido = _db.Pedidos
            .Include(p => p.Itens)
            .ThenInclude(ip => ip.Produto)
            .AsNoTracking()
            .FirstOrDefault(p => p.Id == idPedido);
        return View(pedido);
    }

    private void CarregarProdutos(int? idProduto = null)
    {
        var produtos = _db.Produtos.OrderBy(c => c.Nome).AsNoTracking().ToList();
        var produtosSelectList = new SelectList(
            produtos, "Id", "Nome", idProduto);
        ViewBag.Produtos = produtosSelectList;
    }

    [HttpGet]
    public IActionResult Cadastrar(int idPedido)
    {
        CarregarProdutos();
        var itemPedido = new ItemPedidoModel();
        itemPedido.IdPedido = idPedido;
        return View(itemPedido);
    }

    [HttpPost]
    public IActionResult Cadastrar(ItemPedidoModel itemPedido, int idProduto)
    {
        if (!ModelState.IsValid)
        {
            CarregarProdutos(itemPedido.IdProduto);
            return View(itemPedido);
        }

        var pedido = _db.Pedidos
            .Include(p => p.Itens)
            .FirstOrDefault(p => p.Id == itemPedido.IdPedido);

        var itemPedidoOriginal = pedido.Itens
            .FirstOrDefault(ip => ip.IdProduto == itemPedido.IdProduto);

        if (pedido is null)
        {
            return RedirectToAction("Index", "Pedido");
        }

        var produto = _db.Produtos.Find(itemPedido.IdProduto);

        if (itemPedidoOriginal is null)
        {
            itemPedido.ValorUnitario = produto.Preco.Value;
            pedido.Itens.Add(itemPedido);
            pedido.ValorTotal = pedido.Itens.Sum(ip => ip.ValorItem);
            pedido.DataHora = DateTime.Now;
            _db.SaveChanges();
            return RedirectToAction("Index", new { IdPedido = pedido.Id });
        }
        else
        {
            itemPedidoOriginal.Quantidade = (itemPedidoOriginal.Quantidade + itemPedido.Quantidade);
            pedido.ValorTotal = pedido.Itens.Sum(ip => ip.ValorItem);
            pedido.DataHora = DateTime.Now;
            _db.SaveChanges();
            return RedirectToAction("Index", new { idPedido = pedido.Id });
        }
    }

    [HttpGet]
    public IActionResult Alterar(int idPedido, int idProduto)
    {
        var pedido = _db.Pedidos
            .Include(p => p.Itens)
            .ThenInclude(ip => ip.Produto)
            .FirstOrDefault(p => p.Id == idPedido);

        var itemPedido = pedido.Itens.FirstOrDefault(ip => ip.IdProduto == idProduto);
        if (itemPedido is null)
        {
            return RedirectToAction("Index", new { idPedido });
        }
        return View(itemPedido);
    }

    [HttpPost]
    public IActionResult Alterar(ItemPedidoModel itemPedido)
    {
        var pedido = _db.Pedidos
            .Include(p => p.Itens)
            .FirstOrDefault(p => p.Id == itemPedido.IdPedido);

        var itemPedidoOriginal = pedido.Itens
            .FirstOrDefault(ip => ip.IdProduto == itemPedido.IdProduto);

        if (itemPedidoOriginal is null)
        {
            return RedirectToAction("Index", new { itemPedido.IdPedido });
        }

        if (!ModelState.IsValid)
        {
            return View(itemPedido);
        }

        itemPedidoOriginal.Quantidade = itemPedido.Quantidade;
        pedido.ValorTotal = pedido.Itens.Sum(ip => ip.ValorItem);
        _db.SaveChanges();
        return RedirectToAction("Index", new { itemPedido.IdPedido });
    }

    [HttpGet]
    public IActionResult Excluir(int idPedido, int idProduto)
    {
        var pedido = _db.Pedidos
            .Include(p => p.Itens)
            .ThenInclude(ip => ip.Produto)
            .FirstOrDefault(p => p.Id == idPedido);

        var itemPedido = pedido.Itens.FirstOrDefault(ip => ip.IdProduto == idProduto);
        if (itemPedido is null)
        {
            return RedirectToAction("Index", new { idPedido });
        }
        return View(itemPedido);
    }

    [HttpPost]
    public IActionResult ProcessarExclusao(int idPedido, int idProduto)
    {
        var pedido = _db.Pedidos
            .Include(p => p.Itens)
            .FirstOrDefault(p => p.Id == idPedido);

        var itemPedido = pedido.Itens.FirstOrDefault(ip => ip.IdProduto == idProduto);

        if (itemPedido is null)
        {
            return RedirectToAction("Index", new { idPedido });
        }
        pedido.Itens.Remove(itemPedido);
        pedido.ValorTotal = pedido.Itens.Sum(ip => ip.ValorItem);
        _db.SaveChanges();
        return RedirectToAction("Index", new { idPedido });
    }
}