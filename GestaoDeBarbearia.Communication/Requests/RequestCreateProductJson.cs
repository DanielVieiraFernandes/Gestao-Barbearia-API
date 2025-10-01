using System.ComponentModel.DataAnnotations;

namespace GestaoDeBarbearia.Communication.Requests;
public class RequestCreateProductJson
{
    [Required(ErrorMessage = "O nome do produto é obrigatório")]
    [StringLength(255, MinimumLength = 3, ErrorMessage = "O nome do produto deve ter no mínimo 3 caracteres e no máximo 255")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "O preço do produto é obrigatório")]
    public long SalePrice { get; set; }

    [Required(ErrorMessage = "O custo do produto é obrigatório")]
    public long UnitCost { get; set; }

    [Required(ErrorMessage = "A quantidade do produto é obrigatória")]
    public long Quantity { get; set; }

    [Required(ErrorMessage = "O estoque mínimo do produto é obrigatório")]
    public int MinimumStock { get; set; }
}
