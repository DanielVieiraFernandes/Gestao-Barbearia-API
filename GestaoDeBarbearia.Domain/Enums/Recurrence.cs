using System.ComponentModel;

namespace GestaoDeBarbearia.Domain.Enums;
public enum Recurrence
{
    [Description("Despesa única")]
    Single = 1,
    [Description("Despesa parcelada")]
    Installments = 2,
}
