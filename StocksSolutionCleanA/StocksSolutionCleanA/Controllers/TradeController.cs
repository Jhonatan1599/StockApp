using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Rotativa.AspNetCore;
using ServiceContracts.DTO;
using ServiceContracts.FinnhubService;
using ServiceContracts.StocksService;
using Services;
using StocksApp.Filters.ActionFilters;
using StocksApp.Models;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;

namespace StocksApp.Controllers
{
    [Route("Trade")]
    public class TradeController : Controller
    {
        private readonly TradingOptions _tradingOptions;
        private readonly IFinnhubStockPriceQuoteService _finnhubStockPriceQuoteService;
        readonly IFinnhubSearchStocksService _finnhubSearchStocksService;
        private readonly IFinnhubCompanyProfileService _finnhubCompanyProfileService;


        private readonly IConfiguration _configuration;
        private readonly IStocksBuyOrderService _stocksBuyOrderService;
        private readonly IStocksSellOrderService _stocksSellOrderService;


        public TradeController(IOptions<TradingOptions> tradingOptions, IConfiguration configuration, IStocksBuyOrderService stocksBuyOrderService, IStocksSellOrderService stocksSellOrderService ,  IFinnhubCompanyProfileService finnhubCompanyProfileService, IFinnhubSearchStocksService finnhubSearchStocksService, IFinnhubStockPriceQuoteService finnhubStockPriceQuoteService)
        {
            _tradingOptions = tradingOptions.Value;
            _configuration = configuration;
            _stocksBuyOrderService = stocksBuyOrderService;
            _stocksSellOrderService = stocksSellOrderService;

            _finnhubCompanyProfileService = finnhubCompanyProfileService;
            _finnhubSearchStocksService = finnhubSearchStocksService;
            _finnhubStockPriceQuoteService = finnhubStockPriceQuoteService;
        }

        [Route("[action]/{stockSymbol}")]
        [Route("~/[controller]/{stockSymbol}")]
        public async Task<IActionResult> Index(string stockSymbol)
        {
            //reset stock symbol if not exists
            if (string.IsNullOrEmpty(stockSymbol))
                stockSymbol = "MSFT";

            //get company profile from API server
            Dictionary<string, object>? companyProfileDictionary = await _finnhubCompanyProfileService.GetCompanyProfile(stockSymbol);

            //get stock price quotes fromo API server
            Dictionary<string, object>? stockQuoteDictionary = await _finnhubStockPriceQuoteService.GetStockPriceQuote(stockSymbol);

            //create model object
            StockTrade stockTrade = new StockTrade() { StockName = stockSymbol };

            //load data from finnHubService into model object
            if (companyProfileDictionary != null && stockQuoteDictionary != null)
            {
                stockTrade.StockSymbol = companyProfileDictionary["ticker"].ToString();
                stockTrade.StockName = companyProfileDictionary["name"].ToString();
                stockTrade.Quantity = _tradingOptions.DefaultOrderQuantity ?? 0;
                stockTrade.Price = Convert.ToDouble(stockQuoteDictionary["c"].ToString());
            }

            // Sent Finnhub token to view
            ViewBag.FinnhubToken = _configuration["FinnhubToken"];

            return View(stockTrade);
        }

        [Route("BuyOrder")]
        [HttpPost]
        [TypeFilter(typeof(CreateOrderActionFilter))]
        public async Task<IActionResult> BuyOrder(BuyOrderRequest orderRequest)
        {

            ////update date of order
            //buyOrderRequest.DateAndTimeOfOrder = DateTime.Now;

            ////re-validate the model object after updating the date
            //ModelState.Clear();
            //TryValidateModel(buyOrderRequest);

            //if (!ModelState.IsValid)
            //{
            //    ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            //    StockTrade stockTrade = new StockTrade() { StockName = buyOrderRequest.StockName, Quantity = buyOrderRequest.Quantity, StockSymbol = buyOrderRequest.StockSymbol };
            //    return View("Index", stockTrade);
            //}

            //invoke service method
            BuyOrderResponse buyOrderResponse = await _stocksBuyOrderService.CreateBuyOrder(orderRequest);

            return RedirectToAction(nameof(Orders));
        }

        [Route("SellOrder")]
        [HttpPost]
        [TypeFilter(typeof(CreateOrderActionFilter))]
        public async Task<IActionResult> SellOrder(SellOrderRequest orderRequest)
        {
            ////update date of order
            //sellOrderRequest.DateAndTimeOfOrder = DateTime.Now;

            ////re-validate the model object after updating the date
            //ModelState.Clear();
            //TryValidateModel(sellOrderRequest);

            //if (!ModelState.IsValid)
            //{
            //    ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            //    StockTrade stockTrade = new StockTrade() { StockName = sellOrderRequest.StockName, Quantity = sellOrderRequest.Quantity, StockSymbol = sellOrderRequest.StockSymbol };
            //    return View("Index", stockTrade);
            //}

            //invoke service method
            SellOrderResponse sellOrderResponse = await _stocksSellOrderService.CreateSellOrder(orderRequest);

            return RedirectToAction(nameof(Orders));
        }

        [Route("Orders")]
        public async Task<ActionResult> Orders()
        {
            //invoke service methods
            List<BuyOrderResponse> buyOrderResponses = await _stocksBuyOrderService.GetBuyOrders();
            List<SellOrderResponse> sellOrderResponses = await _stocksSellOrderService.GetSellOrders();

            //create model object
            Orders orders = new Orders() { BuyOrders = buyOrderResponses, SellOrders = sellOrderResponses };

            ViewBag.TradingOptions = _tradingOptions;

            return View(orders);
        }

        [Route("OrdersPDF")]
        public async Task<IActionResult> OrdersPDF()
        {
            //Get list of orders
            List<IOrderResponse> orders = new List<IOrderResponse>();
            orders.AddRange(await _stocksBuyOrderService.GetBuyOrders());
            orders.AddRange(await _stocksSellOrderService.GetSellOrders());
            orders = orders.OrderByDescending(temp => temp.DateAndTimeOfOrder).ToList();

            ViewBag.TradingOptions = _tradingOptions;

            //Return view as pdf
            return new ViewAsPdf("OrdersPDF", orders, ViewData)
            {
                PageMargins = new Rotativa.AspNetCore.Options.Margins() { Top = 20, Right = 20, Bottom = 20, Left = 20 },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
            };
        }

    }
}
