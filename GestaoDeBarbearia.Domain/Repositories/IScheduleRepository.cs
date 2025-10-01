using GestaoDeBarbearia.Domain.Entities;
using GestaoDeBarbearia.Domain.Pagination;

namespace GestaoDeBarbearia.Domain.Repositories;
public interface IScheduleRepository
{
    public Task<Appointment?> FindById(long id);
    public Task<bool> IsTimeSlotOccupied(DateTime appointmentDateTime, long employeeId);
    public Task<List<Appointment>> FindAll(RequestAppointmentsPaginationParamsJson pagination);
    public Task Create(Appointment appointment, List<long> ServiceIds);
    public Task Update(Appointment appointment);
}
