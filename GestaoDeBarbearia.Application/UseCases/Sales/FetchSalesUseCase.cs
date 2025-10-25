using GestaoDeBarbearia.Communication.Responses;
using GestaoDeBarbearia.Domain.Pagination.Sales;
using GestaoDeBarbearia.Domain.Repositories;

namespace GestaoDeBarbearia.Application.UseCases.Sales;
public class FetchSalesUseCase
{
    private readonly ISalesRepository salesRepository;

    public FetchSalesUseCase(ISalesRepository salesRepository)
    {
        this.salesRepository = salesRepository;
    }
    public async Task<ResponseSalesJson> Execute(RequestSalesPaginationParamsJson pagination)
    {
        var sales = await salesRepository.GetAll(pagination);

        return new ResponseSalesJson { sales = sales };
    }

}
