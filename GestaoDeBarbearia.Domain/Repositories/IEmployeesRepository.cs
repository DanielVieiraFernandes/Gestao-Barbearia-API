using GestaoDeBarbearia.Domain.Entities;

namespace GestaoDeBarbearia.Domain.Repositories;
public interface IEmployeesRepository
{
    Task Create(Employee employee);
}
