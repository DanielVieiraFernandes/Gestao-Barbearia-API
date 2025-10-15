using System.ComponentModel;

namespace GestaoDeBarbearia.Domain.Enums;
public enum ExpenseStatus
{
    [Description("Paga")]
    Paid = 1,
    [Description("Pendente de pagamento")]
    Pending = 2,
    [Description("Pagamento agendado")]
    Scheduled = 3,
    [Description("Pagamento atrasado")]
    Overdue = 4,
    [Description("Faltam parcelas")]
    InstallmentsRemaining = 5,
}
