using GestaoDeBarbearia.Communication.Requests;
using GestaoDeBarbearia.Communication.Responses;
using GestaoDeBarbearia.Domain.Entities;
using GestaoDeBarbearia.Domain.Repositories;
using GestaoDeBarbearia.Exception.ExceptionsBase;

namespace GestaoDeBarbearia.Application.UseCases.Appointments;
public class ScheduleServiceUseCase
{
    private ISchedulesRepository scheduleRepository;
    private IServicesRepository servicesRepository;
    public ScheduleServiceUseCase(ISchedulesRepository scheduleRepository, IServicesRepository servicesRepository)
    {
        this.scheduleRepository = scheduleRepository;
        this.servicesRepository = servicesRepository;
    }

    public async Task<ResponseScheduledServiceJson> Execute(RequestScheduleServiceJson request)
    {
        List<Service> services = await servicesRepository.FindAll(request.ServiceIds);

        if (services.Count == 0)
            throw new NotFoundException("Serviço(s) não encontrado(s)");

        // Calcula o tempo de duração com base nos serviços selecionados
        int durationOfService = services.Sum(s => s.DurationMinutes);

        bool isOcupied = await scheduleRepository.IsTimeSlotOccupied(request.AppointmentDateTime, durationOfService, request.EmployeeId);

        if (isOcupied)
            throw new AppointmentTimeBusyException("Esse horário está ocupado!");

        Appointment appointment = new()
        {
            AppointmentDateTime = request.AppointmentDateTime,
            AppointmentEndDateTime = request.AppointmentDateTime.AddMinutes(durationOfService),
            ClientId = request.ClientId,
            ClientName = request.ClientName,
            ClientPhone = request.ClientPhone,
            EmployeeId = request.EmployeeId,
            PaymentType = request.PaymentType
        };

        await scheduleRepository.Create(appointment, services);

        return new ResponseScheduledServiceJson
        {
            AppointmentDateTime = appointment.AppointmentDateTime,
            ServicePrice = appointment.ServicePrice,
            EmployeeId = appointment.EmployeeId,
        };
    }
}
