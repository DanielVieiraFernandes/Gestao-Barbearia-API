using System.ComponentModel;

namespace GestaoDeBarbearia.Domain.Enums;
public enum EmployeePosition
{
    [Description("Barbeiro")]
    Barber = 1,

    [Description("Gerente")]
    Manager = 2,

    [Description("Recepcionista")]
    Receptionist = 3,

}
