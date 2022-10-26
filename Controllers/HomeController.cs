using System.Linq;
using Aula5.Data;
using Microsoft.AspNetCore.Mvc;

namespace Aula5.Controllers;
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