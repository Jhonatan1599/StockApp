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
    public class StocksBuyOrderService : IStocksBuyOrderService
    {
        //private field
        //private readonly ApplicationDbContext _dbContext;

        private readonly IStocksRepository _stocksRepository;
        /// <summary>
        /// Constructor of StocksService class that executes when a new object is created for the class
        /// </summary>
        public StocksBuyOrderService(IStocksRepository stocksRepository)
        {
            _stocksRepository = stocksRepository;
        }
   
        public async Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? buyOrderRequest)
        {
            //Validation: buyOrderRequest can't be null
            if (buyOrderRequest == null) throw new ArgumentNullException(nameof(buyOrderRequest));

            //Model validation
            Validationhelper.ModelValidation(buyOrderRequest);

            //convert buyOrderRequest into BuyOrder type
            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();

            //generate BuyOrderID
            buyOrder.BuyOrderID = Guid.NewGuid();

            //add buy order object to buy orders list
            //_dbContext.BuyOrders.Add(buyOrder);
            //await _dbContext.SaveChangesAsync();
            await _stocksRepository.AddBuyOrder(buyOrder);

            //convert the BuyOrder object into BuyOrderResponse type
            return buyOrder.ToBuyOrderResponse();
        }



        public async Task<List<BuyOrderResponse>> GetBuyOrders()
        {
            //Convert all BuyOrder objects into BuyOrderResponse objects
            //List<BuyOrder> buyOrders = await  _dbContext.BuyOrders
            // .OrderByDescending(temp => temp.DateAndTimeOfOrder)
            // .ToListAsync();
            List<BuyOrder> buyOrders = await _stocksRepository.GetAllBuyOrders();

            return buyOrders.Select(temp => temp.ToBuyOrderResponse()).ToList();
        }

    }
}
