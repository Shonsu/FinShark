
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
    }
}