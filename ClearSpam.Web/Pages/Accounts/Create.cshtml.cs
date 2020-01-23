using AutoMapper;
using ClearSpam.Application.Accounts.Commands;
using ClearSpam.Application.Exceptions;
using ClearSpam.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace ClearSpam.Web.Pages.Accounts
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;

        public CreateModel(IMediator mediator, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            this.mediator = mediator;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
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
                command.UserId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                Account = await mediator.Send(command, CancellationToken.None);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }

            return RedirectToPage("./Edit", new { Account.Id });
        }
    }
}