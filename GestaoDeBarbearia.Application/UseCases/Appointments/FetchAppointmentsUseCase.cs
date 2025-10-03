using GestaoDeBarbearia.Communication.Responses;
using GestaoDeBarbearia.Domain.Pagination.Appointments;
using GestaoDeBarbearia.Domain.Repositories;

namespace GestaoDeBarbearia.Application.UseCases.Appointments;
public class FetchAppointmentsUseCase
{
    private ISchedulesRepository scheduleRepository;
    public FetchAppointmentsUseCase(ISchedulesRepository scheduleRepository)
    {
        this.scheduleRepository = scheduleRepository;
    }
    public async Task<ResponseAppointmentsJson> Execute(RequestAppointmentsPaginationParamsJson pagination)
    {
        var appointments = await scheduleRepository.FindAll(pagination);

        return new ResponseAppointmentsJson { Appointments = appointments };
    }
}
