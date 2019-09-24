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
    public class UpdateRuleCommandHandler : UpdateEntityCommandHandler<Rule, RuleDto>,
                                             IRequestHandler<UpdateRuleCommand, RuleDto>
    {
        public UpdateRuleCommandHandler(IRepository repository, IMapper mapper) :
            base(repository, mapper)
        { }

        public async Task<RuleDto> Handle(UpdateRuleCommand request, CancellationToken cancellationToken)
        {
            return Handle(request);
        }
    }
}