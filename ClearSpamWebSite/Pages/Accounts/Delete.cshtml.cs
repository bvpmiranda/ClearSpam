using ClearSpam.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;

namespace ClearSpam.Pages.Accounts
{
    [Authorize]
    public class DeleteAccount : PageModel
    {
        private readonly ClearSpamContext _context;

        public DeleteAccount(ClearSpamContext context)
        {
            _context = context ?? throw new System.ArgumentNullException(nameof(context));
        }

        [BindProperty]
        public Account Account { get; set; }

        public IActionResult OnGet(int? id)
        {
            Account = _context.Accounts.Where(x => x.Id == id).FirstOrDefault();

            if (Account == null)
                return NotFound();

            Account.OriginalPassword = Account.Password;

            return Page();
        }

        public IActionResult OnPost(int? id)
        {
            var account = _context.Accounts.Where(x => x.Id == id).FirstOrDefault();

            if (account == null)
                return NotFound();

            _context.Remove(account);
            _context.SaveChanges();

            return RedirectToPage("./Index");
        }
    }
}
