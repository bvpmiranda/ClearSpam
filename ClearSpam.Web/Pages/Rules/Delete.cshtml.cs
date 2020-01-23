using ClearSpam.Application.Rules.Commands;
using ClearSpam.Application.Rules.Queries;
using ClearSpam.Application.Exceptions;
using ClearSpam.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading;
using System.Threading.Tasks;
using ClearSpam.Application.Accounts.Queries;
using Microsoft.AspNetCore.Authorization;

namespace ClearSpam.Web.Pages.Rules
{
    [Authorize]
    public class DeleteRuleModel : PageModel
    {
        private readonly IMediator mediator;

        public DeleteRuleModel(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [BindProperty]
        public RuleDto Rule { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            try
            {
                var query = new GetRuleQuery(id.GetValueOrDefault());
                Rule = await mediator.Send(query, CancellationToken.None);

                var accountQuery = new GetAccountQuery(Rule.AccountId);
                Rule.Account = await mediator.Send(accountQuery, CancellationToken.None);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            int accountId;
            try
            {
                var query = new GetRuleQuery(id.GetValueOrDefault());
                accountId = (await mediator.Send(query, CancellationToken.None)).AccountId;

                var command = new DeleteRuleCommand(id.GetValueOrDefault());
                await mediator.Send(command, CancellationToken.None);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }

            return RedirectToPage("../Accounts/Details", new { id = accountId });
        }
    }
}
