using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Controllers;
using api.Controllers.Dto;
using api.Dto.Stock;
using api.Models;

namespace api.Interfaces
{
    public interface IStockRepository
    {
        Task<PagedList<Stock>> GetAllAsync(QueryStockObject query);
        Task<Stock?> GetByIdAsync(int id);  //FirstOrDefault
        Task<Stock?> GetBySymbolAsync(string symbol);
        Task<Stock> CreateAsync(Stock stockModel);
        Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto stockDto);
        Task<Stock?> DeleteAsync(int id);
        Task<bool> StockExist(int id);
    }
}