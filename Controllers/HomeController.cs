using System.Linq;
using controleEstoque.Data;
using Microsoft.AspNetCore.Mvc;

namespace controleEstoque.Controllers;
public class HomeController : Controller
{
    private readonly AppDbContext _db;
    public HomeController(AppDbContext db)
    {
        _db = db;
    }
    public IActionResult Index()
    {

        return View();
    }

}