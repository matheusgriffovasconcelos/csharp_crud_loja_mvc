using System.ComponentModel.DataAnnotations.Schema;

namespace Aula5.Models;

public class ItemPedidoModel
{
    [ForeignKey("Pedido")]
    public int IdPedido { get; set; }
    public PedidoModel Pedido { get; set; }

    [ForeignKey("Produto")]
    public int IdProduto { get; set; }
    public ProdutoModel Produto { get; set; }

    public int Quantidade { get; set; }

    public double ValorUnitario { get; set; }

    [NotMapped]
    public double ValorItem
    {
        get { return this.Quantidade * this.ValorUnitario; }
    }
}