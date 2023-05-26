using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.StocksService
{
    /// <summary>
    /// Represents Stocks service that includes operations like buy order, sell order
    /// </summary>
    public interface IStocksSellOrderService
    {
        /// <summary>
        /// Creates a sell order
        /// </summary>
        /// <param name="sellOrderRequest">Sell order object</param>
        Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? sellOrderRequest);


        /// <summary>
        /// Returns all existing sell orders
        /// </summary>
        /// <returns>Returns a list of objects of SellOrder type</returns>
        Task<List<SellOrderResponse>> GetSellOrders();
    }
}
