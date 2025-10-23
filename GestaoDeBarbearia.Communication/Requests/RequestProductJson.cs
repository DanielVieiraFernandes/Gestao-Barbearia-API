using System.ComponentModel.DataAnnotations;

namespace GestaoDeBarbearia.Communication.Requests;
public class RequestProductJson
{
    [Required(ErrorMessage = "O nome do produto é obrigatório")]
    [StringLength(255, MinimumLength = 3, ErrorMessage = "O nome do produto deve ter no mínimo 3 caracteres e no máximo 255")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "O preço do produto é obrigatório")]
    [Range(0.1, (double)decimal.MaxValue, ErrorMessage = "Preço inválido! o preço deve ser maior que zero e não passar de 15 dígitos")]
    public decimal SalePrice { get; set; }

    [Required(ErrorMessage = "O custo do produto é obrigatório")]
    [Range(0.1, (double)decimal.MaxValue, ErrorMessage = "Custo inválido! o custo deve ser maior que zero e não passar de 15 dígitos")]
    public decimal UnitCost { get; set; }

    [Required(ErrorMessage = "A quantidade do produto é obrigatória")]
    public long Quantity { get; set; }

    [Required(ErrorMessage = "O estoque mínimo do produto é obrigatório")]
    public int MinimumStock { get; set; }
}
