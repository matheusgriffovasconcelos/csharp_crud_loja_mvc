using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aula5.Models;

public class ProdutoModel
{
    [Key]
    [Display(Name = "Código")]
    public int Id { get; set; }

    [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório.")]
    [StringLength(64, ErrorMessage = "O campo {0} comporta até {1} caracteres apenas.")]
    [Display(Name = "Nome")]
    public string Nome { get; set; }

    [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório.")]
    [Display(Name = "Preço")]
    [Range(0, 100000, ErrorMessage = "O campo {0} deve estar entre {1} e {2}.")]
    public double? Preco { get; set; }

    [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório.")]
    [Display(Name = "Estoque")]
    [Range(0, 999, ErrorMessage = "O campo {0} deve estar entre {1} e {2}.")]
    public int? Estoque { get; set; }

    [ForeignKey("Categoria")]
    [Required(ErrorMessage = "O campo {0} é de preenchimento obrigatório.")]
    [Display(Name = "Categoria")]
    public int? IdCategoria { get; set; }

    [Display(Name = "Categoria")]
    public CategoriaModel Categoria { get; set; }
}