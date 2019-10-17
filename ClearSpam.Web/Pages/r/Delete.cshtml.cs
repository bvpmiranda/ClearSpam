using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ClearSpam.Domain.Entities;
using ClearSpam.Persistence;

namespace ClearSpam.Web.Pages.r
{
    public class DeleteModel : PageModel
    {
        private readonly ClearSpam.Persistence.ClearSpamContext _context;

        public DeleteModel(ClearSpam.Persistence.ClearSpamContext context)
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
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Rule = await _context.Rules.FindAsync(id);

            if (Rule != null)
            {
                _context.Rules.Remove(Rule);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
