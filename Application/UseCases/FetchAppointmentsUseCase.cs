using GestaoDeBarbearia.Communication.Responses;
using GestaoDeBarbearia.Domain.Pagination;
using GestaoDeBarbearia.Domain.Repositories;

namespace GestaoDeBarbearia.Application.UseCases;
public class FetchAppointmentsUseCase
{
    private IScheduleRepository scheduleRepository;
    public FetchAppointmentsUseCase(IScheduleRepository scheduleRepository)
    {
        this.scheduleRepository = scheduleRepository;
    }
    public async Task<ResponseAppointmentsJson> Execute(RequestPaginationParamsJson pagination)
    {
        var appointments = await scheduleRepository.FindAll(pagination);

        return new ResponseAppointmentsJson { Appointments = appointments };
    }
}
