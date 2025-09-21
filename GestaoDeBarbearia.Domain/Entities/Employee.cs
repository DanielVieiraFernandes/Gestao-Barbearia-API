using GestaoDeBarbearia.Domain.Enums;

namespace GestaoDeBarbearia.Domain.Entities;
public class Employee
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Telephone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public long Salary { get; set; }
    public EmployeePosition Position { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
