using ClearSpam.Application.Accounts.Queries;
using ClearSpam.Application.Models;
using ClearSpam.Persistence;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace ClearSpam.Web.Pages.Accounts
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IMediator mediator;
        private readonly IHttpContextAccessor httpContextAccessor;

        public IndexModel(IMediator mediator, IHttpContextAccessor httpContextAccessor)
        {
            this.mediator = mediator;
            this.httpContextAccessor = httpContextAccessor;
        }

        public IList<AccountDto> Account { get; set; }

        public async Task OnGetAsync()
        {
            var userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var query = new GetAccountsQuery(userId);
            Account = (await mediator.Send(query, CancellationToken.None)).ToList();
        }

        public IActionResult OnPostClearSpam()
        {
            var userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            Program.ClearSpamService.ProcessRules(userId);

            return RedirectToPage("./Index");
        }
    }
}
