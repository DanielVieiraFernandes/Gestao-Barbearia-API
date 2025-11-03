using GestaoDeBarbearia.Domain.Enums;

namespace GestaoDeBarbearia.Domain.Entities;
public class Employee : User
{
    public decimal Salary { get; set; }
    public EmployeePosition Position { get; set; }
}
