using Stocks.Core.Domain.Entities;

namespace Stocks.Core.Domain.RepositoryContracts
{
    public interface IStocksRepository
    {
        Task<BuyOrder> AddBuyOrder(BuyOrder buyOrder);

        Task<SellOrder> AddSellOrder(SellOrder sellOrder);

        Task<List<BuyOrder>> GetAllBuyOrders();

        Task<List<SellOrder>> GetAllSellOrders();

    }
}