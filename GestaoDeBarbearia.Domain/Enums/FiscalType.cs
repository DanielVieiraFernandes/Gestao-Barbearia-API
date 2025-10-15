using System.ComponentModel;

namespace GestaoDeBarbearia.Domain.Enums;
public enum FiscalType
{
    [Description("Produto")]
    Product = 1,
    [Description("Serviço")]
    Service = 2
}
