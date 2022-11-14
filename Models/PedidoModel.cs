using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace controleEstoque.Models;

public class PedidoModel
{
    [Key]
    public int Id { get; set; }
    public DateTime DataHora { get; set; }
    public double ValorTotal { get; set; }
    public EnumStatusPedido StatusPedido { get; set; }

    public ICollection<ItemPedidoModel> Itens { get; set; }

    [ForeignKey("Cliente")]
    [Display(Name = "Cliente")]
    public int IdCliente { get; set; }
    public ClienteModel Cliente { get; set; }
}