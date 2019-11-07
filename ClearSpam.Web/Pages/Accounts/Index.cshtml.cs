using ClearSpam.Application.Accounts.Queries;
using ClearSpam.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ClearSpam.Web.Pages.Accounts
{
    public class IndexModel : PageModel
    {
        private readonly IMediator mediator;

        public IndexModel(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public IList<AccountDto> Account { get; set; }

        public async Task OnGetAsync()
        {
            var query = new GetAccountsQuery();
            Account = (await mediator.Send(query, CancellationToken.None)).ToList();
        }

        public IActionResult OnPostClearSpam()
        {
            Program.ClearSpamService.Restart();

            return RedirectToPage("./Index");
        }
    }
}
