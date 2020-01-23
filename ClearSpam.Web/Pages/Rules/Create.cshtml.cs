using AutoMapper;
using ClearSpam.Application.Accounts.Queries;
using ClearSpam.Application.Exceptions;
using ClearSpam.Application.Fields.Queries;
using ClearSpam.Application.Models;
using ClearSpam.Application.Rules.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ClearSpam.Web.Pages.Rules
{
    [Authorize]
    public class CreateRuleModel : PageModel
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public CreateRuleModel(IMediator mediator, IMapper mapper)
        {
            this.mediator = mediator;
            this.mapper = mapper;
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            try
            {
                Rule = new RuleDto();

                await FillMissingProperties(id.GetValueOrDefault());
            }
            catch (NotFoundException)
            {
                return NotFound();
            }

            return Page();
        }

        [BindProperty]
        public RuleDto Rule { get; set; }

        public IEnumerable<FieldDto> Fields { get; private set; }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var command = mapper.Map<CreateRuleCommand>(Rule);
                var rule = await mediator.Send(command, CancellationToken.None);

                Program.ClearSpamService.ProcessRules(rule.AccountId, rule.Id);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (ValidationException e)
            {
                foreach (var failure in e.Failures)
                {
                    ModelState.AddModelError(failure.Key, string.Join(';', failure.Value));
                }

                await FillMissingProperties(Rule.AccountId);

                return Page();
            }

            return RedirectToPage("../Accounts/Details", new { id = Rule.AccountId });
        }

        private async Task FillMissingProperties(int accountId)
        {
            if (Rule.Account == null)
            {
                var accountQuery = new GetAccountQuery(accountId);
                Rule.Account = await mediator.Send(accountQuery, CancellationToken.None);
            }

            if (Fields == null)
            {
                var fieldsQuery = new GetFieldsQuery();
                Fields = (await mediator.Send(fieldsQuery, CancellationToken.None));
            }
        }
    }
}