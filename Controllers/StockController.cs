
using api.Data;
using api.Dto.Stock;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult GetAll()
        {
            var stocks = _db.Stocks.ToList();
            IEnumerable<StockDto> enumerable = stocks.Select(s => s.ToStockDto());
            return Ok(enumerable);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var stock = _db.Stocks.Find(id);
            if (stock == null)
            {
                return NotFound();
            }
            return Ok(stock.ToStockDto());
        }
        [HttpPost]
        public IActionResult Create([FromBody] CreateStockRequestDto stockDto)
        {
            var stockModel = stockDto.ToStock();
            _db.Stocks.Add(stockModel);
            _db.SaveChanges();
            return CreatedAtAction(
                nameof(GetById),
                new { id = stockModel.Id },
                stockModel.ToStockDto()
                );
        }
        [HttpPut("{id}")]
        //  [Route("{Id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto)
        {
            var stockModel = _db.Stocks.FirstOrDefault(x => x.Id == id);
            if (stockModel == null)
            {
                return NotFound();
            }
            _db.Entry(stockModel).CurrentValues.SetValues(updateDto);
            _db.SaveChanges();
            return Ok(stockModel.ToStockDto());
        }
        [HttpDelete("{id}")]
        public IActionResult DeletyeById(int id)
        {
            var stockModel = _db.Stocks.FirstOrDefault(s => s.Id == id);
            if (stockModel == null)
            {
                return NotFound();
            }
            _db.Stocks.Remove(stockModel);
            return NoContent();
        }
    }
}