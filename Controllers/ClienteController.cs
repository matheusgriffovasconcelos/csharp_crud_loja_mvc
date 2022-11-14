using System.Linq;
using controleEstoque.Data;
using controleEstoque.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
namespace controleEstoque.Controllers;

public class ClienteController : Controller
{
    private readonly AppDbContext _db;

    public ClienteController(AppDbContext db)
    {
        _db = db;
    }

    public IActionResult Index()
    {

        var clientes = _db.Clientes.AsNoTracking().OrderBy(x => x.Nome).ToList();
        return View(clientes);
    }

    [HttpGet]
    public IActionResult Cadastrar()
    {
        var cliente = new ClienteModel();
        return View(cliente);
    }

    [HttpPost]
    public IActionResult Cadastrar(ClienteModel cliente)
    {
        if (!ModelState.IsValid)
        {
            return View(cliente);
        }
        _db.Clientes.Add(cliente);
        _db.SaveChanges();
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Alterar(int id)
    {

        var cliente = _db.Clientes.Find(id);
        if (cliente is null)
        {
            return RedirectToAction("Index");
        }
        return View(cliente);
    }

    [HttpPost]
    public IActionResult Alterar(int id, ClienteModel cliente)
    {
        var clienteOriginal = _db.Clientes.Find(id);
        if (clienteOriginal is null)
        {
            return RedirectToAction("Index");
        }

        if (!ModelState.IsValid)
        {
            return View(cliente);
        }
        clienteOriginal.Nome = cliente.Nome;
        clienteOriginal.Email = cliente.Email;
        clienteOriginal.Telefone = cliente.Telefone;
        clienteOriginal.DataNascimento = cliente.DataNascimento;
        clienteOriginal.Dependentes = cliente.Dependentes;
        clienteOriginal.LimiteCredito = cliente.LimiteCredito;
        clienteOriginal.Vip = cliente.Vip;
        _db.SaveChanges();
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Excluir(int id)
    {
        var clientes = _db.Clientes.Find(id);
        if (clientes is null)
        {
            return RedirectToAction("Index");
        }
        return View(clientes);
    }

    [HttpPost]
    public IActionResult ProcessarExclusao(int id)
    {
        var clienteOriginal = _db.Clientes.Find(id);
        if (clienteOriginal is null)
        {
            return RedirectToAction("Index");
        }

        _db.Clientes.Remove(clienteOriginal);
        _db.SaveChanges();
        return RedirectToAction("Index");
    }
}