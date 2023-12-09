using ClearSpam;
using ClearSpam.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ClearSpam.Web.Pages.Rules
{
    [Authorize]
    public class DeleteRule : PageModel
    {
        private readonly ClearSpamContext _context;

        public DeleteRule(ClearSpamContext context)
        {
            _context = context ?? throw new System.ArgumentNullException(nameof(context));
        }

        [BindProperty]
        public Rule Rule { get; set; }

        public IActionResult OnGet(int? id)
        {
            Rule = _context.Rules.Where(x => x.Id == id).Include(x => x.Account).FirstOrDefault();

            if (Rule == null)
                return NotFound();

            return Page();
        }

        public IActionResult OnPost(int? id)
        {
            var rule = _context.Rules.Where(x => x.Id == id).FirstOrDefault();
            var accountId = rule.AccountId;

            if (rule == null)
                return NotFound();

            _context.Remove(rule);
            _context.SaveChanges();

            return RedirectToPage("../Accounts/Details", new { id = accountId });
        }
    }
}
