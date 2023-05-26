using Stocks.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// DTO class that represents a sell order - that can be used as return type of Stocks service
    /// </summary>
    public class SellOrderResponse:IOrderResponse
    {
        public Guid SellOrderID { get; set; }

        public string StockSymbol { get; set; }

        public string StockName { get; set; }

        public DateTime DateAndTimeOfOrder { get; set; }

        public uint Quantity { get; set; }

        public double Price { get; set; }

        public double TradeAmount { get; set; }

        public OrderType TypeOfOrder => OrderType.SellOrder;

        /// <summary>
        /// Checks if the current object and other (parameter) object values match
        /// </summary>
        /// <param name="obj">Other object of BuyOrderResponse class, to compare</param>
        /// <returns>True or false determines whether current object and other objects match</returns>
        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj is not SellOrderResponse) return false;

            SellOrderResponse other = (SellOrderResponse)obj;
            return SellOrderID == other.SellOrderID && StockSymbol == other.StockSymbol && StockName == other.StockName && DateAndTimeOfOrder == other.DateAndTimeOfOrder && Quantity == other.Quantity && Price == other.Price;
        }

        /// <summary>
        /// Returns an int value that represents unique stock id of the current object
        /// </summary>
        /// <returns>unique int value</returns>
        public override int GetHashCode()
        {
            return StockSymbol.GetHashCode();
        }

        /// <summary>
        /// Converts the current object into string which includes the values of all properties
        /// </summary>
        /// <returns>A string with values of all properties of current object</returns>
        public override string ToString()
        {
            return $"Buy Order ID: {SellOrderID}, Stock Symbol: {StockSymbol}, Stock Name: {StockName}, Date and Time of Buy Order: {DateAndTimeOfOrder.ToString("dd MMM yyyy hh:mm ss tt")}, Quantity: {Quantity}, Buy Price: {Price}, Trade Amount: {TradeAmount}";
        }
    }

    public static class SellOrderExtentions
    {
        /// <summary>
        /// An extension method to convert an object of SellOrder class into SellOrderResponse class
        /// </summary>
        /// <param name="sellOrder"></param>
        /// <returns>Returns the converted SellOrderResponse object</returns>
        public static SellOrderResponse ToSellOrderResponse(this SellOrder sellOrder)
        {   
           
            return new SellOrderResponse()
            {
                SellOrderID=sellOrder.SellOrderID,StockName = sellOrder.StockName, StockSymbol= sellOrder.StockSymbol, DateAndTimeOfOrder=sellOrder.DateAndTimeOfOrder,Quantity=sellOrder.Quantity, Price=sellOrder.Price,TradeAmount =sellOrder.Price *sellOrder.Quantity
            };
        }
    }
}
