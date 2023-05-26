using ServiceContracts.DTO;
using ServiceContracts.StocksService;
using Services.Helpers;
using Stocks.Core.Domain.Entities;
using Stocks.Core.Domain.RepositoryContracts;



/*Implement the above service interface called 'IStocksService' that performs the specified operation.
 
CreateBuyOrder: Inserts a new buy order into the database table called 'BuyOrders'.
CreateSellOrder: Inserts a new sell order into the database table called 'SellOrders'.
GetBuyOrders: Returns the existing list of buy orders retrieved from database table called 'BuyOrders'.
GetSellOrders: Returns the existing list of sell orders retrieved from database table called 'SellOrders'.*/
namespace Services.StocksService
{
    public class StocksSellOrderService : IStocksSellOrderService
    {
        //private field
        //private readonly ApplicationDbContext _dbContext;

        private readonly IStocksRepository _stocksRepository;
        /// <summary>
        /// Constructor of StocksService class that executes when a new object is created for the class
        /// </summary>
        public StocksSellOrderService(IStocksRepository stocksRepository)
        {
            _stocksRepository = stocksRepository;
        }



        public async Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? sellOrderRequest)
        {

            if (sellOrderRequest == null) throw new ArgumentNullException(nameof(sellOrderRequest));

            Validationhelper.ModelValidation(sellOrderRequest);

            // Convert to SellOrder From sellOrderRequest
            SellOrder newSellOrder = sellOrderRequest.ToSellOrder();

            // Gnerate a SellOrder ID
            newSellOrder.SellOrderID = Guid.NewGuid();

            //Add to list
            //_dbContext.SellOrders.Add(newSellOrder);
            //await _dbContext.SaveChangesAsync();
            await _stocksRepository.AddSellOrder(newSellOrder);
            // Return SellOrderResponse
            return newSellOrder.ToSellOrderResponse();
        }



        public async Task<List<SellOrderResponse>> GetSellOrders()
        {
            //Convert all SellOrder objects into SellOrderResponse objects
            //List<SellOrder> sellOrders = await _dbContext.SellOrders
            // .OrderByDescending(temp => temp.DateAndTimeOfOrder)
            // .ToListAsync();
            List<SellOrder> sellOrders = await _stocksRepository.GetAllSellOrders();

            return sellOrders.Select(temp => temp.ToSellOrderResponse()).ToList();
        }
    }
}
