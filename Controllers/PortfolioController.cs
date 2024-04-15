using api.Extensions;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/portfolio")]
    public class PortfolioController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IStockRepository _stockRepo;
        private readonly IPortfolioRepository _portfolioRepository;

        public PortfolioController(UserManager<AppUser> userManager, IStockRepository stockRepo, IPortfolioRepository portfolioRepository)
        {
            _userManager = userManager;
            _stockRepo = stockRepo;
            _portfolioRepository = portfolioRepository;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserPortfolio()
        {
            string userEmail = User.GetEmail();
            AppUser? appUser = await _userManager.FindByEmailAsync(userEmail);
            var userPortfolios = await _portfolioRepository.GetUserPortfolios(appUser);
            return Ok(userPortfolios);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateUserPortfolio(string symbol)
        {
            var userEmail = User.GetEmail();
            var appUser = await _userManager.FindByEmailAsync(userEmail);

            var stock = await _stockRepo.GetBySymbolAsync(symbol);
            if (stock == null)
            {
                return BadRequest("Stock not found");
            }

            var userPortfolios = await _portfolioRepository.GetUserPortfolios(appUser);

            if (userPortfolios.Any(s => s.Id == stock.Id))
            {
                return BadRequest(string.Format("Stock {0} already in user portfolios", stock.Symbol));
            }

            var portfolioModel = new Portfolio
            {
                AppUserId = appUser.Id,
                StockId = stock.Id
            };

            await _portfolioRepository.CreateAsync(portfolioModel);
            if (portfolioModel == null)
            {
                return StatusCode(500, "Could not created");
            }

            return Created();
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeletePortfolio(string symbol)
        {
            var userEmail = User.GetEmail();
            var appUser = await _userManager.FindByEmailAsync(userEmail);

            var userPortfolios = await _portfolioRepository.GetUserPortfolios(appUser);

            var filteredStock = userPortfolios.Where(s => s.Symbol == symbol).ToList();
            if (filteredStock.Count() == 1)
            {
                await _portfolioRepository.DeleteAsync(appUser, symbol);
            }
            else
            {
                return BadRequest($"{appUser.UserName} doesnt have {symbol} stock");
            }
            return Ok();
        }
    }
}