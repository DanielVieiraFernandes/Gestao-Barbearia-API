using System.ComponentModel.DataAnnotations;

namespace GestaoDeBarbearia.Communication.Requests;
public class RequestRegisterSaleJson
{
    [Required(ErrorMessage = "É obrigatório informar a data da venda.")]
    public DateTime SaleDate { get; set; }

    [MinLength(1, ErrorMessage = "É necessário pelo menos 1 item na venda.")]
    public List<RequestDetailsProductSaleJson> DetailsProductSale { get; set; } = default!;
}
