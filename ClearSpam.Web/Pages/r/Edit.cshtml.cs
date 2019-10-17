using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClearSpam.Domain.Entities;
using ClearSpam.Persistence;

namespace ClearSpam.Web.Pages.r
{
    public class EditModel : PageModel
    {
        private readonly ClearSpam.Persistence.ClearSpamContext _context;

        public EditModel(ClearSpam.Persistence.ClearSpamContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Rule Rule { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Rule = await _context.Rules
                .Include(r => r.Account).FirstOrDefaultAsync(m => m.Id == id);

            if (Rule == null)
            {
                return NotFound();
            }
           ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Login");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Rule).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RuleExists(Rule.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool RuleExists(int id)
        {
            return _context.Rules.Any(e => e.Id == id);
        }
    }
}
