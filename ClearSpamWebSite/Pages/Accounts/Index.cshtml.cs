using ClearSpam;
using ClearSpam.Entities;
using ClearSpam.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Identity.Client;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ClearSpam.Pages.Accounts
{
    [Authorize]
    public class AccountsIndex : PageModel
    {
        private readonly ClearSpamContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IClearSpamService _clearSpamService;

        public AccountsIndex(ClearSpamContext context, IHttpContextAccessor httpContextAccessor, IClearSpamService clearSpamService)
        {
            _context = context ?? throw new System.ArgumentNullException(nameof(context));
            _httpContextAccessor = httpContextAccessor ?? throw new System.ArgumentNullException(nameof(httpContextAccessor));
            _clearSpamService = clearSpamService ?? throw new System.ArgumentNullException(nameof(clearSpamService));
        }

        public IList<Account> Accounts { get; set; }

        public async Task OnGetAsync()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            Accounts = _context.Accounts.Where(x => x.UserId == userId).ToList();
        }

        public async Task<IActionResult> OnPostClearSpam()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            await _clearSpamService.ProcessRulesAsync(userId);

            return RedirectToPage("./Index");
        }
    }
}
