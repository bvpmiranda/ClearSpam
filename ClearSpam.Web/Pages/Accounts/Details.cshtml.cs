using ClearSpam.Application.Accounts.Queries;
using ClearSpam.Application.Exceptions;
using ClearSpam.Application.Models;
using ClearSpam.Application.Rules.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ClearSpam.Web.Pages.Accounts
{
    public class DetailsModel : PageModel
    {
        private readonly IMediator mediator;

        public DetailsModel(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public AccountDto Account { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            try
            {
                var accountQuery = new GetAccountQuery(id.GetValueOrDefault());
                Account = await mediator.Send(accountQuery, CancellationToken.None);

                var rulesQuery = new GetRulesQuery(id.GetValueOrDefault());
                Account.Rules = (await mediator.Send(rulesQuery, CancellationToken.None)).ToHashSet();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
