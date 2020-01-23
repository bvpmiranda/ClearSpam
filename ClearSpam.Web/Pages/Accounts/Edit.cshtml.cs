using AutoMapper;
using ClearSpam.Application.Accounts.Commands;
using ClearSpam.Application.Accounts.Queries;
using ClearSpam.Application.Exceptions;
using ClearSpam.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ClearSpam.Web.Pages.Accounts
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public EditModel(IMediator mediator, IMapper mapper)
        {
            this.mediator = mediator;
            this.mapper = mapper;
        }

        [BindProperty]
        public AccountDto Account { get; set; }
        public IEnumerable<string> Mailboxes { get; private set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            try
            {
                var accountQuery = new GetAccountQuery(id.GetValueOrDefault());
                Account = await mediator.Send(accountQuery, CancellationToken.None);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }

            try
            {
                var mailboxesQuery = new GetMailboxesQuery(Account);
                Mailboxes = await mediator.Send(mailboxesQuery, CancellationToken.None);
            }
            catch (System.Exception)
            {
                Mailboxes = null;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var command = mapper.Map<UpdateAccountCommand>(Account);
                await mediator.Send(command, CancellationToken.None);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }

            return RedirectToPage("./Index");
        }
    }
}
