using controleEstoque.Data;
using controleEstoque.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace controleEstoque.Controllers;

public class PedidoController : Controller
{
    private readonly AppDbContext _db;

    public PedidoController(AppDbContext db)
    {
        _db = db;
    }

    private void CarregarClientes(int? idCliente = null)
    {
        var clientes = _db.Clientes.OrderBy(c => c.Nome).ToList();
        var clientesSelectList = new SelectList(
            clientes, "Id", "Nome", idCliente);
        ViewBag.Clientes = clientesSelectList;
    }
    public IActionResult Index()
    {
        var pedidos = _db.Pedidos
            .Include(p => p.Cliente)
            .AsNoTracking()
            .OrderByDescending(c => c.DataHora)
            .ToList();
        return View(pedidos);
    }

    [HttpGet]
    public IActionResult Cadastrar()
    {
        CarregarClientes();
        var pedido = new PedidoModel();
        return View(pedido);
    }

    [HttpPost]
    public IActionResult Cadastrar(PedidoModel pedido)
    {
        pedido.DataHora = DateTime.Now;
        pedido.StatusPedido = EnumStatusPedido.Realizado;
        if (!ModelState.IsValid)
        {
            CarregarClientes(pedido.IdCliente);
            return View(pedido);
        }
        _db.Pedidos.Add(pedido);
        _db.SaveChanges();
        return RedirectToAction("Cadastrar", "ItemPedido", new ItemPedidoModel { IdPedido = pedido.Id });
    }

    [HttpGet]
    public IActionResult Alterar(int id)
    {
        var pedido = _db.Pedidos.Find(id);
        if (pedido is null)
        {
            return RedirectToAction("Index");
        }
        CarregarClientes(pedido.IdCliente);
        return View(pedido);
    }

    [HttpPost]
    public IActionResult Alterar(int id, PedidoModel pedido)
    {
        var pedidoOriginal = _db.Pedidos.Find(id);
        if (pedidoOriginal is null)
        {
            return RedirectToAction("Index");
        }

        if (!ModelState.IsValid)
        {
            CarregarClientes(pedido.IdCliente);
            return View(pedido);
        }
        pedidoOriginal.IdCliente = pedido.IdCliente;
        _db.SaveChanges();
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Excluir(int id)
    {
        var pedido = _db.Pedidos.Find(id);
        if (pedido is null)
        {
            return RedirectToAction("Index");
        }
        return View(pedido);
    }

    [HttpPost]
    public IActionResult ProcessarExclusao(int id)
    {
        var pedidoOriginal = _db.Pedidos.Find(id);
        if (pedidoOriginal is null)
        {
            return RedirectToAction("Index");
        }
        _db.Pedidos.Remove(pedidoOriginal);
        _db.SaveChanges();
        return RedirectToAction("Index");
    }

    public IActionResult ProgredirStatus(int id)
    {
        var pedido = _db.Pedidos.Find(id);
        if (pedido is null)
        {
            return RedirectToAction("Index");
        }
        var intStatus = (int)pedido.StatusPedido;
        if (intStatus < 6)
            pedido.StatusPedido = (EnumStatusPedido)(intStatus + 1);
        _db.SaveChanges();
        return RedirectToAction("Index");
    }

    public IActionResult RegredirStatus(int id)
    {
        var pedido = _db.Pedidos.Find(id);
        if (pedido is null)
        {
            return RedirectToAction("Index");
        }
        var intStatus = (int)pedido.StatusPedido;
        if (intStatus > 0)
            pedido.StatusPedido = (EnumStatusPedido)(intStatus - 1);
        _db.SaveChanges();
        return RedirectToAction("Index");
    }

    public IActionResult Cancelar(int id)
    {
        var pedido = _db.Pedidos.Find(id);
        if (pedido is null)
        {
            return RedirectToAction("Index");
        }
        pedido.StatusPedido = EnumStatusPedido.Cancelado;
        _db.SaveChanges();
        return RedirectToAction("Index");
    }
}