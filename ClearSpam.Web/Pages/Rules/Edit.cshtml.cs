﻿using AutoMapper;
using ClearSpam.Application.Accounts.Queries;
using ClearSpam.Application.Exceptions;
using ClearSpam.Application.Fields.Queries;
using ClearSpam.Application.Models;
using ClearSpam.Application.Rules.Commands;
using ClearSpam.Application.Rules.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ClearSpam.Web.Pages.Rules
{
    public class EditRuleModel : PageModel
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public EditRuleModel(IMediator mediator, IMapper mapper)
        {
            this.mediator = mediator;
            this.mapper = mapper;
        }

        [BindProperty]
        public RuleDto Rule { get; set; }

        public IEnumerable<FieldDto> Fields { get; private set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            try
            {
                var ruleQuery = new GetRuleQuery(id.GetValueOrDefault());
                Rule = await mediator.Send(ruleQuery, CancellationToken.None);

                var accountQuery = new GetAccountQuery(id.GetValueOrDefault());
                Rule.Account = await mediator.Send(accountQuery, CancellationToken.None);

                var fieldsQuery = new GetFieldsQuery();
                Fields = (await mediator.Send(fieldsQuery, CancellationToken.None));
            }
            catch (NotFoundException)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var command = mapper.Map<UpdateRuleCommand>(Rule);
                await mediator.Send(command, CancellationToken.None);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }

            return RedirectToPage("../Accounts/Details", new { id = Rule.AccountId });
        }
    }
}
