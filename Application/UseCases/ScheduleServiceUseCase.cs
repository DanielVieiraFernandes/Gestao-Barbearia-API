using GestaoDeBarbearia.Communication.Requests;
using GestaoDeBarbearia.Communication.Responses;
using GestaoDeBarbearia.Domain.Entities;
using GestaoDeBarbearia.Domain.Repositories;
using GestaoDeBarbearia.Exception.ExceptionsBase;

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
        bool isOcupied = await scheduleRepository.IsTimeSlotOccupied(request.AppointmentDateTime, request.EmployeeId);

        if (isOcupied)
            throw new AppointmentTimeBusyException("Esse horário está ocupado!");

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

        await scheduleRepository.Create(appointment, request.ServiceIds);

        return new ResponseScheduledServiceJson
        {
            AppointmentDateTime = request.AppointmentDateTime,
            ServicePrice = request.ServicePrice,
            EmployeeId = request.EmployeeId
        };
    }
}
