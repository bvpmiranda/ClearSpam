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
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public EditModel(IMediator mediator, IMapper mapper)
        {
            this._mediator = mediator;
            this._mapper = mapper;
        }

        [BindProperty]
        public AccountDto Account { get; set; }
        public IEnumerable<string> Mailboxes { get; private set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            try
            {
                var accountQuery = new GetAccountQuery(id.GetValueOrDefault());
                Account = await _mediator.Send(accountQuery, CancellationToken.None);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }

            try
            {
                var mailboxesQuery = new GetMailboxesQuery(Account);
                Mailboxes = await _mediator.Send(mailboxesQuery, CancellationToken.None);
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
                var command = _mapper.Map<UpdateAccountCommand>(Account);
                await _mediator.Send(command, CancellationToken.None);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }

            return RedirectToPage("./Index");
        }
    }
}
