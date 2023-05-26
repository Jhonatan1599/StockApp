//Package
//using Microsoft.Extensions.Configuration;

//internal
using Exceptions;
using ServiceContracts.FinnhubService;
using Stocks.Core.Domain.RepositoryContracts;

namespace Services.FinnhubService
{
    public class FinnhubStocksService : IFinnhubStocksService
    {
        private readonly IFinnhubRepository _finnhubRepository;


        public FinnhubStocksService(IFinnhubRepository finnhubRepository)
        {
            _finnhubRepository = finnhubRepository;
        }


        public async Task<List<Dictionary<string, string>>?> GetStocks()
        {
            try
            {
                //invoke repository
                List<Dictionary<string, string>>? responseDictionary = await _finnhubRepository.GetStocks();

                //return response dictionary back to the caller
                return responseDictionary;
            }
            catch (Exception ex)
            {
                FinnhubException finnhubException = new FinnhubException("Unable to connect to finnhub", ex);
                throw finnhubException;
            }
        }



    }
}

