using ClearSpam.Entities;
using ClearSpam.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace ClearSpam.Pages.Accounts
{
    [Authorize]
    public class CreateAccount : PageModel
    {
        private readonly ClearSpamContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICryptography _cryptography;

        public CreateAccount(ClearSpamContext context, IHttpContextAccessor httpContextAccessor, ICryptography cryptography)
        {
            _context = context ?? throw new System.ArgumentNullException(nameof(context));
            _httpContextAccessor = httpContextAccessor ?? throw new System.ArgumentNullException(nameof(httpContextAccessor));
            _cryptography = cryptography ?? throw new System.ArgumentNullException(nameof(cryptography));
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Account Account { get; set; }

        public IActionResult OnPost()
        {
            Account.UserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            Account.Password = _cryptography.Encrypt(Account.Password);

            _context.Add(Account);
            _context.SaveChanges();

            return RedirectToPage("./Edit", new { Account.Id });
        }
    }
}