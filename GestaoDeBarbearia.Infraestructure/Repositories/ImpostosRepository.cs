using Dapper;
using GestaoDeBarbearia.Domain.Entities;
using GestaoDeBarbearia.Domain.Enums;
using GestaoDeBarbearia.Domain.Repositories;
using GestaoDeBarbearia.Infraestructure.Utils;
using System.Text;

namespace GestaoDeBarbearia.Infraestructure.Repositories;
public class ImpostosRepository : IImpostosRepository
{
    private readonly DatabaseQueryBuilder dbFunctions;
    public ImpostosRepository(DatabaseQueryBuilder dbFunctions)
    {
        this.dbFunctions = dbFunctions;
    }
    public async Task<ImpostoSimplesNacional> GetByType(FiscalType fiscalType, decimal billing)
    {
        await using var connecion = await dbFunctions.CreateNewConnection();

        StringBuilder sql = new();

        if (fiscalType == FiscalType.Product)
            sql.Append("SELECT * FROM barber_shop_impostos_simples_nacional WHERE anexo = 'Anexo I' AND ");
        else if (fiscalType == FiscalType.Service)
            sql.Append("SELECT * FROM barber_shop_impostos_simples_nacional WHERE anexo = 'Anexo III' AND ");

        sql.Append("(faturamentolimiteinferior <= @Billing AND faturamentolimitesuperior >= @Billing)");

        var result = await connecion.QuerySingleAsync<ImpostoSimplesNacional>(sql.ToString(), new
        {
            Billing = billing
        });

        return result;
    }
}
