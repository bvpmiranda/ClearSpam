using AutoMapper;
using ClearSpam.Application.Accounts.Commands;
using ClearSpam.Application.Exceptions;
using ClearSpam.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading;
using System.Threading.Tasks;

namespace ClearSpam.Web.Pages.Accounts
{
    public class CreateModel : PageModel
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public CreateModel(IMediator mediator, IMapper mapper)
        {
            this.mediator = mediator;
            this.mapper = mapper;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public AccountDto Account { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var command = mapper.Map<CreateAccountCommand>(Account);
                Account = await mediator.Send(command, CancellationToken.None);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }

            return RedirectToPage("./Edit", new { Id = Account.Id });
        }
    }
}