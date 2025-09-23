using GestaoDeBarbearia.Domain.Entities;

namespace GestaoDeBarbearia.Domain.Repositories;
public interface IScheduleRepository
{
    public Task<Appointment?> FindById(long id);
    public Task<bool> IsTimeSlotOccupied(DateTime appointmentDateTime, long employeeId);
    public Task<List<Appointment>> FindAll();
    public Task Create(Appointment appointment, List<long> ServiceIds);
    public Task Update(Appointment appointment);
}
