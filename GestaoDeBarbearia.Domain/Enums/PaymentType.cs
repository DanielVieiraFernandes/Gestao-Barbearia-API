using System.ComponentModel;

namespace GestaoDeBarbearia.Domain.Enums;

public enum PaymentType
{
    [Description("Cartão de Crédito")]
    CreditCard = 1,

    [Description("Cartão de Débito")]
    DebitCard = 2,

    [Description("Dinheiro")]
    Cash = 3,

    [Description("Pix")]
    Pix = 4,

    [Description("Outro")]
    Other = 5,
}
