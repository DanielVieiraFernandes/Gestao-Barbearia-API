using GestaoDeBarbearia.Domain.Entities;
using GestaoDeBarbearia.Domain.Pagination.Appointments;

namespace GestaoDeBarbearia.Domain.Repositories;
public interface ISchedulesRepository
{
    public Task<Appointment?> FindById(long id);
    public Task<bool> IsTimeSlotOccupied(DateTime appointmentDateTime, int durationOfService, long employeeId);
    public Task<List<Appointment>> FindAll(RequestAppointmentsPaginationParamsJson pagination);
    public Task<List<Appointment>> FilterByMonth(DateOnly month);
    public Task Create(Appointment appointment, List<Service> services);
    public Task Update(Appointment appointment);
}
