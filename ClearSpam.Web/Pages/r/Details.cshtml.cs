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
    public class DetailsModel : PageModel
    {
        private readonly ClearSpam.Persistence.ClearSpamContext _context;

        public DetailsModel(ClearSpam.Persistence.ClearSpamContext context)
        {
            _context = context;
        }

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
    }
}
