using GestaoDeBarbearia.Domain.Entities;
using GestaoDeBarbearia.Domain.Enums;
using GestaoDeBarbearia.Domain.Repositories;
using GestaoDeBarbearia.Exception.ExceptionsBase;

namespace GestaoDeBarbearia.Application.UseCases.Appointments;
public class RejectAnAppointmentUseCase
{
    private readonly ISchedulesRepository schedulesRepository;

    public RejectAnAppointmentUseCase(ISchedulesRepository schedulesRepository)
    {
        this.schedulesRepository = schedulesRepository;
    }

    public async Task Execute(long appointmentId)
    {
        Appointment? appointment = await schedulesRepository.FindById(appointmentId);

        if (appointment is null)
            throw new NotFoundException("Agendamento não encontrado!");

        if (appointment.Status != AppointmentStatus.Pending)
            throw new Exception.ExceptionsBase.
                InvalidOperationException(@"O Agendamento não está pendente de confirmação, portanto, não pode ser rejeitado.");

        appointment.Status = AppointmentStatus.Rejected;

        await schedulesRepository.Update(appointment);
    }
}
