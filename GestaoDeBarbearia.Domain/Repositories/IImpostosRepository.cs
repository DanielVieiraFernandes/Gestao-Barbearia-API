using GestaoDeBarbearia.Domain.Entities;
using GestaoDeBarbearia.Domain.Enums;

namespace GestaoDeBarbearia.Domain.Repositories;
public interface IImpostosRepository
{
    Task<ImpostoSimplesNacional> GetByType(FiscalType fiscalType, decimal billing);
}
