using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ClearSpam.Domain.Entities;
using ClearSpam.Persistence;

namespace ClearSpam.Web.Pages.r
{
    public class CreateModel : PageModel
    {
        private readonly ClearSpam.Persistence.ClearSpamContext _context;

        public CreateModel(ClearSpam.Persistence.ClearSpamContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Login");
            return Page();
        }

        [BindProperty]
        public Rule Rule { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Rules.Add(Rule);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}