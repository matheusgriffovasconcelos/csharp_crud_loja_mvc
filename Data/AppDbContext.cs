using controleEstoque.Models;
using Microsoft.EntityFrameworkCore;

namespace controleEstoque.Data;
public class AppDbContext : DbContext
{
    public DbSet<CategoriaModel> Categorias { get; set; }
    public DbSet<ClienteModel> Clientes { get; set; }
    public DbSet<ProdutoModel> Produtos { get; set; }
    public DbSet<PedidoModel> Pedidos { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<ItemPedidoModel>().HasKey(ip => new { ip.IdPedido, ip.IdProduto });
    }
}