using GestaoDeBarbearia.Communication.Requests;
using GestaoDeBarbearia.Communication.Responses;
using GestaoDeBarbearia.Domain.Entities;
using GestaoDeBarbearia.Domain.Repositories;

namespace GestaoDeBarbearia.Application.UseCases;
public class ScheduleServiceUseCase
{
    private IScheduleRepository scheduleRepository;
    public ScheduleServiceUseCase(IScheduleRepository scheduleRepository)
    {
        this.scheduleRepository = scheduleRepository;
    }

    public async Task<ResponseScheduledServiceJson> Execute(RequestScheduleServiceJson request)
    {
        Appointment appointment = new()
        {
            AppointmentDateTime = request.AppointmentDateTime,
            ClientId = request.ClientId,
            ClientName = request.ClientName,
            ClientPhone = request.ClientPhone,
            EmployeeId = request.EmployeeId,
            ServicePrice = request.ServicePrice,
            PaymentType = request.PaymentType
        };

        await scheduleRepository.Create(appointment);

        return new ResponseScheduledServiceJson
        {
            AppointmentDateTime = request.AppointmentDateTime,
            ServicePrice = request.ServicePrice,
            EmployeeId = request.EmployeeId
        };
    }
}
