using Microsoft.AspNetCore.Mvc;
using BankAPI.Models;

namespace BankAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly Bank _bank;

        public AccountsController(Bank bank)
        {
            _bank = bank;
        }

        [HttpGet]
        public ActionResult<List<BankAccount>> GetAccounts()
        {
            return Ok(_bank.GetAllAccounts());
        }

        [HttpGet("{id}")]
        public ActionResult<BankAccount> GetAccount(int id)
        {
            var account = _bank.GetAccountById(id);
            if (account == null)
                return NotFound("Account not found.");
            return Ok(account);
        }

        [HttpPost]
        public ActionResult<BankAccount> CreateAccount([FromBody] CreateAccountRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Owner))
                return BadRequest("Owner name is required.");

            var account = _bank.CreateAccount(request.Owner);
            return CreatedAtAction(nameof(GetAccount), new { id = account.AccountID }, account);
        }

        [HttpPost("{id}/deposit")]
        public IActionResult Deposit(int id, [FromBody] TransactionRequest request)
        {
            bool result = _bank.Deposit(id, request.Amount);
            if (!result)
                return BadRequest("Deposit failed.");
            return Ok("Deposit successful.");
        }

        [HttpPost("{id}/withdraw")]
        public IActionResult Withdraw(int id, [FromBody] TransactionRequest request)
        {
            bool result = _bank.Withdraw(id, request.Amount);
            if (!result)
                return BadRequest("Withdraw failed.");
            return Ok("Withdraw successful.");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAccount(int id)
        {
            bool result = _bank.DeleteAccount(id);
            if (!result)
                return NotFound("Account not found or could not be deleted.");
            return Ok("Account deleted.");
        }
    }

    public class CreateAccountRequest
    {
        public string Owner { get; set; }
    }

    public class TransactionRequest
    {
        public decimal Amount { get; set; }
    }
}
