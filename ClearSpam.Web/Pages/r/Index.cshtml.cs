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
    public class IndexModel : PageModel
    {
        private readonly ClearSpam.Persistence.ClearSpamContext _context;

        public IndexModel(ClearSpam.Persistence.ClearSpamContext context)
        {
            _context = context;
        }

        public IList<Rule> Rule { get;set; }

        public async Task OnGetAsync()
        {
            Rule = await _context.Rules
                .Include(r => r.Account).ToListAsync();
        }
    }
}
