using GestaoDeBarbearia.Domain.Entities;
using GestaoDeBarbearia.Domain.Enums;
using GestaoDeBarbearia.Domain.Repositories;
using GestaoDeBarbearia.Exception.ExceptionsBase;

namespace GestaoDeBarbearia.Application.UseCases.Appointments;
public class CanceledAnAppointmentUseCase
{
    private readonly ISchedulesRepository schedulesRepository;

    public CanceledAnAppointmentUseCase(ISchedulesRepository schedulesRepository)
    {
        this.schedulesRepository = schedulesRepository;
    }

    public async Task Execute(long appointmentId)
    {
        Appointment? appointment = await schedulesRepository.FindById(appointmentId);

        if (appointment is null)
            throw new NotFoundException("Agendamento não encontrado!");

        if (appointment.Status == AppointmentStatus.Completed || appointment.Status == AppointmentStatus.Rejected)
            throw new Exception.ExceptionsBase.
                InvalidOperationException(@"O status do agendamento não permite cancelamento.");

        appointment.Status = AppointmentStatus.Canceled;

        await schedulesRepository.Update(appointment);
    }
}
