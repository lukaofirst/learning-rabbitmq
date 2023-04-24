using MicroRabbit.Banking.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MicroRabbit.Banking.Api.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class BankingController : ControllerBase
	{
		private readonly IAccountService _accountService;

		public BankingController(IAccountService accountService)
		{
			_accountService = accountService;
		}

		[HttpGet]
		public IActionResult Get()
		{
			var result = _accountService.GetAccounts();
			return Ok(result);
		}
	}
}