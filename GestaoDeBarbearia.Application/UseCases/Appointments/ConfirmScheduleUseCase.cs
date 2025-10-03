using GestaoDeBarbearia.Domain.Enums;
using GestaoDeBarbearia.Domain.Repositories;
using GestaoDeBarbearia.Exception.ExceptionsBase;

namespace GestaoDeBarbearia.Application.UseCases.Appointments;
public class ConfirmScheduleUseCase
{
    private ISchedulesRepository scheduleRepository;
    public ConfirmScheduleUseCase(ISchedulesRepository scheduleRepository)
    {
        this.scheduleRepository = scheduleRepository;
    }

    public async Task Execute(long Id)
    {
        var appointment = await scheduleRepository.FindById(Id);

        if (appointment is null)
            throw new NotFoundException("Agendamento não encontrado");

        if (appointment.Status != AppointmentStatus.Pending)
            throw new AppointmentAlreadyConfirmedException("Agendamento já confirmado!");

        appointment.Status = AppointmentStatus.Confirmed;

        await scheduleRepository.Update(appointment);
    }
}
