using MicroRabbit.Banking.Application.Interfaces;
using MicroRabbit.Banking.Application.Models;
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

		[HttpPost]
		public IActionResult Post([FromBody] AccountTransfer accountTransfer)
		{
			_accountService.Transfer(accountTransfer);

			return Ok(accountTransfer);
		}
	}
}