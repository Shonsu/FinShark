using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using api.Controllers;
using api.Controllers.Dto;
using api.Data;
using api.Dto.Stock;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class StockRepository : IStockRepository
    {

        private readonly AppDbContext _context;

        public StockRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Stock> CreateAsync(Stock stockModel)
        {
            await _context.Stocks.AddAsync(stockModel);
            await _context.SaveChangesAsync();
            return stockModel;
        }

        public async Task<Stock?> DeleteAsync(int id)
        {
            var stockModel = await _context.Stocks.FirstOrDefaultAsync(s => s.Id == id);
            if (stockModel == null)
            {
                return null;
            }
            _context.Remove(stockModel);
            await _context.SaveChangesAsync();
            return stockModel;
        }

        public async Task<PagedList<Stock>> GetAllAsync(QueryObject query, QueryPageableParams queryPageable)
        {
            IQueryable<Stock> stocks = _context.Stocks.Include(s => s.Comments).AsNoTracking().AsQueryable();
            if (!string.IsNullOrWhiteSpace(query.CompanyName))
            {
                stocks = stocks.Where(s => s.CompanyName.Contains(query.CompanyName));
            }
            if (!string.IsNullOrWhiteSpace(query.Symbol))
            {
                stocks = stocks.Where(s => s.Symbol.Contains(query.Symbol));
            }
            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                stocks = stocks.OrderByDynamic(query.SortBy, !query.IsDescending ? "ASC" : "DESC");
            }
            return await PagedList<Stock>.ToPagedList(stocks, queryPageable.PageNumber, queryPageable.PageSize);
        }

        public async Task<Stock?> GetByIdAsync(int id)
        {
            return await _context.Stocks.Include(s => s.Comments).AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto stockDto)
        {
            var existingStock = await _context.Stocks.FirstOrDefaultAsync(s => s.Id == id);
            if (existingStock == null)
            {
                return null;
            }
            _context.Entry(existingStock).CurrentValues.SetValues(stockDto);
            await _context.SaveChangesAsync();
            return existingStock;
        }

        public async Task<bool> StockExist(int id)
        {
            return await _context.Stocks.AnyAsync(s => s.Id == id);
        }
    }
}