using GestaoDeBarbearia.Communication.Responses;
using GestaoDeBarbearia.Domain.Repositories;
using GestaoDeBarbearia.Exception.ExceptionsBase;

namespace GestaoDeBarbearia.Application.UseCases.Appointments;
public class GetAppointmentByIdUseCase
{

    private ISchedulesRepository scheduleRepository;
    public GetAppointmentByIdUseCase(ISchedulesRepository scheduleRepository)
    {
        this.scheduleRepository = scheduleRepository;
    }
    public async Task<ResponseAppointmentJson> Execute(long id)
    {
        var appointment = await scheduleRepository.FindById(id);

        if (appointment is null)
            throw new NotFoundException("Agendamento não encontrado");

        return new ResponseAppointmentJson
        {
            appointment = appointment
        };
    }
}
