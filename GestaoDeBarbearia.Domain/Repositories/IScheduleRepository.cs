using GestaoDeBarbearia.Domain.Entities;

namespace GestaoDeBarbearia.Domain.Repositories;
public interface IScheduleRepository
{
    public Task<Appointment?> FindById(long id);
    public Task<List<Appointment>> FindAll();
    public Task Create(Appointment appointment);
    public Task Update();
}
