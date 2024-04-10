
using api.Data;
using api.Dto.Stock;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [ApiController]
    [Route("api/stock")]
    public class StockController : ControllerBase
    {
        private readonly AppDbContext _db;
        public StockController(AppDbContext dbContext)
        {
            _db = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var stocks = await _db.Stocks.ToListAsync();
            var enumerable = stocks.Select(s => s.ToStockDto());
            return Ok(enumerable);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var stock = await _db.Stocks.FindAsync(id);
            if (stock == null)
            {
                return NotFound();
            }
            return Ok(stock.ToStockDto());
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto)
        {
            var stockModel = stockDto.ToStock();
            await _db.Stocks.AddAsync(stockModel);
            await _db.SaveChangesAsync();
            return CreatedAtAction(
                nameof(GetById),
                new { id = stockModel.Id },
                stockModel.ToStockDto()
                );
        }
        [HttpPut("{id}")]
        //  [Route("{Id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto)
        {
            var stockModel = await _db.Stocks.FirstOrDefaultAsync(x => x.Id == id);
            if (stockModel == null)
            {
                return NotFound();
            }
            _db.Entry(stockModel).CurrentValues.SetValues(updateDto);
            await _db.SaveChangesAsync();
            return Ok(stockModel.ToStockDto());
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletyeById(int id)
        {
            var stockModel = await _db.Stocks.FirstOrDefaultAsync(s => s.Id == id);
            if (stockModel == null)
            {
                return NotFound();
            }
            _db.Stocks.Remove(stockModel);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}