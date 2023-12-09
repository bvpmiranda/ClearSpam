using ClearSpam;
using ClearSpam.Entities;
using ClearSpam.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClearSpam.Web.Pages.Rules
{
    [Authorize]
    public class CreateRule : PageModel
    {
        private readonly ClearSpamContext _context;
        private readonly IClearSpamService _clearSpamService;

        public CreateRule(ClearSpamContext context, IClearSpamService clearSpamService)
        {
            _context = context ?? throw new System.ArgumentNullException(nameof(context));
            _clearSpamService = clearSpamService ?? throw new System.ArgumentNullException(nameof(clearSpamService));
        }

        public IActionResult OnGetAsync(int? id)
        {
            Rule = new Rule();

            FillMissingProperties(id.GetValueOrDefault());

            return Page();
        }

        [BindProperty]
        public Rule Rule { get; set; }

        public IEnumerable<Field> Fields { get; private set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var existingRule = await _context.Rules.Where(x => x.Id != Rule.Id &&
                                                               x.AccountId == Rule.AccountId &&
                                                               x.Field == Rule.Field &&
                                                               x.Content == Rule.Content).FirstOrDefaultAsync();

            if (existingRule != null)
            {
                ModelState.AddModelError("Rule.Content", $"A rule for '{Rule.Field}: {Rule.Content}' already exists");

                FillMissingProperties(Rule.AccountId);

                await _clearSpamService.ProcessRuleAsync(existingRule.AccountId, existingRule.Id);
                
                return Page();
            }

            _context.Add(Rule);
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