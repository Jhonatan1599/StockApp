using Entities;
using Microsoft.EntityFrameworkCore;
using Stocks.Core.Domain.Entities;
using Stocks.Core.Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class StocksRepository : IStocksRepository
    {
        private readonly ApplicationDbContext _context;

        public StocksRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<BuyOrder> AddBuyOrder(BuyOrder buyOrder)
        {
            _context.BuyOrders.Add(buyOrder);
            await _context.SaveChangesAsync();
            return buyOrder;
        }

        public async Task<SellOrder> AddSellOrder(SellOrder sellOrder)
        {
            _context.SellOrders.Add(sellOrder);
            await _context.SaveChangesAsync();
            return sellOrder;
        }

        public async Task<List<BuyOrder>> GetAllBuyOrders()
        {
            return await _context.BuyOrders.OrderByDescending(temp => temp.DateAndTimeOfOrder).ToListAsync();

        }

        public async Task<List<SellOrder>> GetAllSellOrders()
        {
            return await (_context.SellOrders.OrderByDescending(temp => temp.DateAndTimeOfOrder).ToListAsync());
        }
    }
}
