using Dapper;
using GestaoDeBarbearia.Domain.Entities;
using GestaoDeBarbearia.Domain.Pagination.Sales;
using GestaoDeBarbearia.Domain.Repositories;
using GestaoDeBarbearia.Exception.ExceptionsBase;
using GestaoDeBarbearia.Infraestructure.Utils;
using System.Text;

namespace GestaoDeBarbearia.Infraestructure.Repositories;
public class SalesRepository : ISalesRepository
{
    private readonly DBFunctions dbFunctions;
    private readonly IProductsRepository productsRepository;
    public SalesRepository(DBFunctions dbFunctions, IProductsRepository productsRepository)
    {
        this.dbFunctions = dbFunctions;
        this.productsRepository = productsRepository;
    }
    public async Task<(Sale, List<SaleDetails>)> Create(Sale sale, List<SaleDetails> saleDetails)
    {
        await using var connection = await dbFunctions.CreateNewConnection();

        List<Product> productsToUpdate = [];

        foreach (var sd in saleDetails)
        {
            var product = await productsRepository.FindById(sd.ProductId);

            if (product is null)
                throw new NotFoundException($"Produto não encontrado");

            if (product.Quantity < sd.Quantity)
                throw new ErrorRegisteringSaleException($"Estoque insuficiente para o produto {product.Name}");

            if (product.Quantity - sd.Quantity < product.MinimumStock)
                throw new ErrorRegisteringSaleException($"Estoque do produto {product.Name} está abaixo do estoque mínimo.");

            product.Quantity -= sd.Quantity;

            productsToUpdate.Add(product);

            sd.UnitSalePrice = product.SalePrice * sd.Quantity;

            sale.SaleTotal += sd.UnitSalePrice;
        }

        using var transaction = await connection.BeginTransactionAsync();

        StringBuilder sql = new();

        sql.Append(DBFunctions.CreateInsertQuery<Sale>("barber_shop_sales"));
        sql.Append(" RETURNING *; ");

        var createdSale = await connection.QuerySingleAsync<Sale>(sql.ToString(), sale);

        if (createdSale is null)
        {
            await transaction.RollbackAsync();
            throw new ErrorRegisteringSaleException("Erro ao registrar venda");
        }

        sql.Clear();

        saleDetails.ForEach(sd => sd.SaleId = createdSale.Id);

        sql.Append(DBFunctions.CreateInsertQuery<SaleDetails>("barber_shop_sale_details"));
        var linesCreatedSaleDetails = await connection.ExecuteAsync(sql.ToString(), saleDetails);

        if (linesCreatedSaleDetails != saleDetails.Count)
        {
            await transaction.RollbackAsync();
            throw new ErrorRegisteringSaleException("Erro ao registrar detalhes da venda");
        }

        var createdSaleDetails = await connection.QueryAsync<SaleDetails>("SELECT * FROM barber_shop_sale_details WHERE saleid = @SaleId;", new { SaleId = createdSale.Id });

        if (createdSaleDetails is null || !createdSaleDetails.Any())
        {
            await transaction.RollbackAsync();
            throw new ErrorRegisteringSaleException("Erro ao registrar detalhes da venda");
        }

        sql.Clear();

        sql.Append(DBFunctions.CreateUpdateQuery<Product>("barber_shop_products"));

        var linesUpdatedProducts = await connection.ExecuteAsync(sql.ToString(), productsToUpdate);

        if (linesUpdatedProducts != productsToUpdate.Count)
        {
            await transaction.RollbackAsync();
            throw new ErrorRegisteringSaleException("Erro ao atualizar estoque dos produtos");
        }

        await transaction.CommitAsync();

        return (createdSale, [.. createdSaleDetails]);
    }

    public async Task<Sale> FindById(long id)
    {
        await using var connection = await dbFunctions.CreateNewConnection();

        string sql = DBFunctions.CreateSelectByIdQuery("barber_shop_sales");

        var sale = await connection.QuerySingleOrDefaultAsync<Sale>(sql, new { Id = id });

        if (sale is null)
            throw new NotFoundException("Venda não encontrada");

        return sale;
    }

    public async Task<List<SaleDetails>> FindDetailsById(long saleId)
    {
        await using var connection = await dbFunctions.CreateNewConnection();

        string sql = DBFunctions.CreateSelectByIdQuery("barber_shop_sale_details", "saleid");

        var saleDetails = await connection.QueryAsync<SaleDetails>(sql, new { SaleId = saleId });

        if (saleDetails is null || !saleDetails.Any())
            throw new NotFoundException("Detalhes da venda não encontrados");

        return [.. saleDetails];
    }

    public Task<List<Sale>> GetAll(RequestSalesPaginationParamsJson? paginationParams = null)
    {
        throw new NotImplementedException();
    }
}
