using ClearSpam.Entities;
using ClearSpam.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using System.Threading.Tasks;

namespace ClearSpam.Pages.Accounts
{
    [Authorize]
    public class AccountDetails : PageModel
    {
        private readonly ClearSpamContext _context;
        private readonly IClearSpamService _clearSpamService;

        public AccountDetails(ClearSpamContext context, IClearSpamService clearSpamService)
        {
            _context = context ?? throw new System.ArgumentNullException(nameof(context));
            _clearSpamService = clearSpamService ?? throw new System.ArgumentNullException(nameof(clearSpamService));
        }

        public Account Account { get; set; }

        public IActionResult OnGet(int? id)
        {
            Account = _context.Accounts.Where(x => x.Id == id).FirstOrDefault();

            if (Account == null)
                return NotFound();

            Account.Rules = _context.Rules.Where(x => x.AccountId == Account.Id).OrderBy(x => x.Field).ThenBy(x => x.Content).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostClearSpam([FromForm] int accountId)
        {
            await _clearSpamService.ProcessRulesAsync(accountId);

            return new OkResult();
        }
    }
}
