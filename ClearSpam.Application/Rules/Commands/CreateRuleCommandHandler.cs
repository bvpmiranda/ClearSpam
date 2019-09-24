using AutoMapper;
using ClearSpam.Application.BaseMediator.Commands;
using ClearSpam.Application.Models;
using ClearSpam.Domain.Entities;
using ClearSpam.Domain.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ClearSpam.Application.Rules.Commands
{
    public class CreateRuleCommandHandler : CreateEntityCommandHandler<Rule, RuleDto>,
                                             IRequestHandler<CreateRuleCommand, RuleDto>
    {
        public CreateRuleCommandHandler(IRepository repository, IMapper mapper) :
            base(repository, mapper)
        { }

        public async Task<RuleDto> Handle(CreateRuleCommand request, CancellationToken cancellationToken)
        {
            return Handle(request);
        }
    }
}