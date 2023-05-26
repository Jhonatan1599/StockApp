using Stocks.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// DTO class that represents a sell order - that can be used while inserting / updating
    /// </summary>
    public class SellOrderRequest:IValidatableObject, IOrderRequest
    {
        [Required(ErrorMessage = "Stock Symbol can't be null or empty")]
        public string StockSymbol { get; set; }

        [Required(ErrorMessage = "Stock Name can't be null or empty")]
        public string StockName { get; set; }  

        public DateTime DateAndTimeOfOrder { get; set; }

        [Range(1, 100000, ErrorMessage = "You can sell maximum of 100000 shares in single order. Minimum is 1.")]
        public uint Quantity { get; set; }

        [Range(1, 10000, ErrorMessage = "The maximum price of stock is 10000. Minimum is 1.")]
        public double Price { get; set; }

        public SellOrder ToSellOrder( )
        {
            return new SellOrder() { StockSymbol = StockSymbol, StockName = StockName, DateAndTimeOfOrder = DateAndTimeOfOrder, Quantity = Quantity, Price = Price };
        }

        /// <summary>
        /// Model class-level validation using IValidatableObject
        /// </summary>
        /// <param name="validationContext">ValidationContext to validate</param>
        /// <returns>Returns validation errors as ValidationResult</returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new List<ValidationResult>();

            //Date of order should be less than Jan 01, 2000
            if (DateAndTimeOfOrder < Convert.ToDateTime("2000-01-01"))
            {
                results.Add(new ValidationResult("Date of the order should not be older than Jan 01, 2000."));
            }

            return results;
        }
    }
}
