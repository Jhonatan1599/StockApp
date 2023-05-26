using System.ComponentModel.DataAnnotations;

namespace Stocks.Core.Domain.Entities
{
    /// <summary>
    /// Represents a buy order to purchase the stocks
    /// </summary>
    public class BuyOrder
    {
        [Key]
        public Guid BuyOrderID { get; set; }

        public string StockSymbol { get; set; }

        [Required(ErrorMessage = "Stock Name can't be null or empty")]
        public string StockName { get; set; }

        public DateTime DateAndTimeOfOrder { get; set; }

        [Range(1, 100000, ErrorMessage = "You can buy maximun of 100000 shares in single order. Minimun is 1.")]
        public uint Quantity { get; set; }

        [Range(1, 10000, ErrorMessage = "The maximun price of stock is 10000. Minimun is 1.")]
        public double Price { get; set; }
    }



}

////These two entity classes are used to create list objects to store records in a service class.
