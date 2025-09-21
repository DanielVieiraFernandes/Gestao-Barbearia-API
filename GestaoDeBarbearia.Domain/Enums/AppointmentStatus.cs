﻿using System.ComponentModel;

namespace GestaoDeBarbearia.Domain.Enums;

public enum AppointmentStatus
{
    [Description("Aguardando confirmação")]
    Pending = 1,

    [Description("Agendamento confirmado")]
    Confirmed = 2,

    [Description("Atendimento concluído")]
    Completed = 3,
}
