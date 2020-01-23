using ClearSpam.Application.Accounts.Commands;
using ClearSpam.Application.Accounts.Queries;
using ClearSpam.Application.Exceptions;
using ClearSpam.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading;
using System.Threading.Tasks;

namespace ClearSpam.Web.Pages.Accounts
{
    public class DeleteModel : PageModel
    {
        private readonly IMediator mediator;

        public DeleteModel(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [BindProperty]
        public AccountDto Account { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            try
            {
                var query = new GetAccountQuery(id.GetValueOrDefault());
                Account = await mediator.Send(query, CancellationToken.None);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            try
            {
                var query = new DeleteAccountCommand(id.GetValueOrDefault());
                await mediator.Send(query, CancellationToken.None);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }

            return RedirectToPage("./Index");
        }
    }
}
