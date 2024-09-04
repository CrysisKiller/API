using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Helpers;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDBcontext _context;
        public StockRepository(ApplicationDBcontext context)
        {
            _context = context;
        }

        public async Task<List<Stock>> GetAllAsync(QueryObject query)
        {
            var stocks = _context.Stock.Include(c => c.MyProperty).ThenInclude(c => c.AppUser).AsQueryable();
            if (!string.IsNullOrWhiteSpace(query.CompanyName))
                stocks = stocks.Where(s => s.CompanyName.Contains(query.CompanyName));

            if (!string.IsNullOrWhiteSpace(query.Symbol))
                stocks = stocks.Where(s => s.Symbol.Contains(query.Symbol));

            if (!string.IsNullOrWhiteSpace(query.SortBy)){
                if(query.SortBy.Equals("Symbol",StringComparison.OrdinalIgnoreCase))
                    stocks = query.IsDescending?stocks.OrderByDescending(s=>s.Symbol):stocks.OrderBy(s=>s.Symbol);
            }
                return await stocks.ToListAsync();
        }
        public async Task<Stock?> GetByIDAsync(int id)
        {
            // var stock =await _context.Stock.FindAsync(id);   

            //any will work Find or the below one  even SingleOrDefault but SingleOrDefault goes through the full list and then return, if it finds multiple items matching the condition throws an exception

            var stock = await _context.Stock.Include(c => c.MyProperty).FirstOrDefaultAsync(stock => stock.Id == id);
            return stock;
        }

        public async Task<Stock> CreateAsync(Stock stock)
        {
            await _context.Stock.AddAsync(stock);
            await _context.SaveChangesAsync();
            return stock;
        }

        public async Task<Stock?> DeteteAsync(int id)
        {
            var stock = await _context.Stock.FirstOrDefaultAsync(stock => stock.Id == id);
            if (stock == null)
                return null;
            _context.Stock.Remove(stock); //remove is not an async function  
            await _context.SaveChangesAsync();
            return stock;
        }
        public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto stockDto)
        {
            var stock = await _context.Stock.FirstOrDefaultAsync(stock => stock.Id == id);
            if (stock == null)
                return null;

            stock.Industry = stockDto.Industry;
            stock.CompanyName = stockDto.CompanyName;
            stock.Purchase = stockDto.Purchase;
            stock.LastDiv = stockDto.LastDiv;
            stock.MarketCap = stockDto.MarketCap;
            stock.Symbol = stockDto.Symbol;

            await _context.SaveChangesAsync();
            return stock;
        }

        public async Task<bool> StockExists(int id)
        {
            return await _context.Stock.AnyAsync(s => s.Id == id);
        }

        public async Task<Stock?> GetBySymbolAsync(string symbol)
        {
            return await _context.Stock.FirstOrDefaultAsync(s=> s.Symbol == symbol);
        }
    }
}