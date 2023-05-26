using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ServiceContracts.FinnhubService;
using ServiceContracts.StocksService;
using StocksApp;

namespace StockMarketSolution.ViewComponents
{
    public class SelectedStockViewComponent : ViewComponent
    {
        private readonly TradingOptions _tradingOptions;
        private readonly IStocksSellOrderService _stocksService;
        private readonly IFinnhubCompanyProfileService _finnhubCompanyProfileService;
        private readonly IFinnhubStockPriceQuoteService _finnhubStockPriceQuoteService;
        private readonly IConfiguration _configuration;


        /// <summary>
        /// Constructor for TradeController that executes when a new object is created for the class
        /// </summary>
        /// <param name="tradingOptions">Injecting TradeOptions config through Options pattern</param>
        /// <param name="stocksService">Injecting StocksService</param>
        /// <param name="finnhubService">Injecting FinnhubService</param>
        /// <param name="configuration">Injecting IConfiguration</param>
        public SelectedStockViewComponent(IOptions<TradingOptions> tradingOptions, IStocksSellOrderService stocksService, IFinnhubStocksService finnhubService, IConfiguration configuration, IFinnhubStockPriceQuoteService finnhubStockPriceQuoteService, IFinnhubCompanyProfileService finnhubCompanyProfileService)
        {
            _tradingOptions = tradingOptions.Value;
            _stocksService = stocksService;
            _configuration = configuration;
            _finnhubCompanyProfileService = finnhubCompanyProfileService;
            _finnhubStockPriceQuoteService = finnhubStockPriceQuoteService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string? stockSymbol)
        {
            Dictionary<string, object>? companyProfileDict = null;

            if (stockSymbol != null)
            {
                companyProfileDict = await _finnhubCompanyProfileService.GetCompanyProfile(stockSymbol);
                var stockPriceDict = await _finnhubStockPriceQuoteService.GetStockPriceQuote(stockSymbol);
                if (stockPriceDict != null && companyProfileDict != null)
                {
                    companyProfileDict.Add("price", stockPriceDict["c"]);
                }
            }

            if (companyProfileDict != null && companyProfileDict.ContainsKey("logo"))
                return View(companyProfileDict);
            else
                return Content("");
        }
    }
}

