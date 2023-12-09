using ClearSpam;
using ClearSpam.Entities;
using ClearSpam.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClearSpam.Web.Pages.Rules
{
    [Authorize]
    public class EditRule : PageModel
    {
        private readonly ClearSpamContext _context;
        private readonly IClearSpamService _clearSpamService;

        public EditRule(ClearSpamContext context, IClearSpamService clearSpamService)
        {
            _context = context ?? throw new System.ArgumentNullException(nameof(context));
            _clearSpamService = clearSpamService ?? throw new System.ArgumentNullException(nameof(clearSpamService));
        }

        [BindProperty]
        public Rule Rule { get; set; }

        public IEnumerable<Field> Fields { get; private set; }

        public ActionResult OnGet(int? id)
        {
            Rule = _context.Rules.Where(x => x.Id == id).FirstOrDefault();

            if (Rule == null)
                return NotFound();

            FillMissingProperties(Rule.AccountId);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (_context.Rules.Any(x => x.Id != Rule.Id &&
                                        x.AccountId == Rule.AccountId &&
                                        x.Field == Rule.Field &&
                                        x.Content == Rule.Content))
            {
                ModelState.AddModelError("Rule.Content", $"A rule for '{Rule.Field}: {Rule.Content}' already exists");

                FillMissingProperties(Rule.AccountId);

                return Page();
            }

            _context.SaveChanges();

            await _clearSpamService.ProcessRuleAsync(Rule.AccountId, Rule.Id);

            return RedirectToPage("../Accounts/Details", new { id = Rule.AccountId });
        }

        private void FillMissingProperties(int accountId)
        {
            if (Rule.Account == null)
                Rule.Account = _context.Accounts.Find(accountId);

            Fields ??= _context.Fields.ToList();
        }
    }
}
