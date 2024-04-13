
using api.Data;
using api.Dto.Stock;
using api.Interfaces;
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
        private readonly IStockRepository _stockRepo;
        public StockController(AppDbContext dbContext, IStockRepository stockRepository)
        {
            _db = dbContext;
            _stockRepo = stockRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] QueryObject query)
        {
            // if (!ModelState.IsValid)
            // {
            //     return BadRequest(ModelState);
            // }
            var stocks = await _stockRepo.GetAllAsync(query);
            var stockDtos = stocks.Select(s => s.ToStockDto());
            return Ok(stockDtos);
        }

        [HttpGet("{stockId:int}")]
        public async Task<IActionResult> GetById(int stockId)
        {
            // if (!ModelState.IsValid)
            // {
            //     return BadRequest(ModelState);
            // }
            Stock? stock = await _stockRepo.GetByIdAsync(stockId);
            if (stock == null)
            {
                return NotFound();
            }
            return Ok(stock.ToStockDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto)
        {
            // if (!ModelState.IsValid)
            // {
            //     return BadRequest(ModelState);
            // }
            Stock stock = await _stockRepo.CreateAsync(stockDto.ToStock());
            return CreatedAtAction(
                nameof(GetById),
                new { stockId = stock.Id },
                stock.ToStockDto()
                );
        }

        [HttpPut("{stockId:int}")]
        //  [Route("{stockId:int}")]
        public async Task<IActionResult> Update([FromRoute] int stockId, [FromBody] UpdateStockRequestDto updateDto)
        {
            // if (!ModelState.IsValid)
            // {
            //     return BadRequest(ModelState);
            // }
            Stock? stock = await _stockRepo.UpdateAsync(stockId, updateDto);
            if (stock == null)
            {
                return NotFound();
            }
            return Ok(stock.ToStockDto());
        }

        [HttpDelete("{stockId:int}")]
        public async Task<IActionResult> DeletyeById(int stockId)
        {
            // if (!ModelState.IsValid)
            // {
            //     return BadRequest(ModelState);
            // }
            var stockModel = await _stockRepo.DeleteAsync(stockId);
            if (stockModel == null)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}