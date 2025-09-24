using GestaoDeBarbearia.Domain.Enums;
using GestaoDeBarbearia.Domain.Repositories;
using GestaoDeBarbearia.Exception.ExceptionsBase;

namespace GestaoDeBarbearia.Application.UseCases;
public class MarkServiceAsCompletedUseCase
{
    private IScheduleRepository scheduleRepository;
    public MarkServiceAsCompletedUseCase(IScheduleRepository scheduleRepository)
    {
        this.scheduleRepository = scheduleRepository;
    }
    public async Task Execute(long Id)
    {
        var appointment = await scheduleRepository.FindById(Id);

        if (appointment is null)
            throw new NotFoundException("Agendamento não encontrado");

        if (appointment.Status != AppointmentStatus.Confirmed)
            throw new AppointmentNotConfirmedException("Erro ao marcar agendamento como concluído");

        appointment.Status = AppointmentStatus.Completed;
        appointment.PaidAt = DateTime.Now;
        appointment.UpdatedAt = DateTime.Now;

        await scheduleRepository.Update(appointment);
    }
}
