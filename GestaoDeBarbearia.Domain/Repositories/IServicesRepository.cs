using GestaoDeBarbearia.Domain.Entities;

namespace GestaoDeBarbearia.Domain.Repositories;
public interface IServicesRepository
{
    Task<List<Service>> FindAll(List<long>? ids = null);
}
