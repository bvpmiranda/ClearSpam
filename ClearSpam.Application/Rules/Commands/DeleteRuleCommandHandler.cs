using ClearSpam.Application.BaseMediator.Commands;
using ClearSpam.Domain.Entities;
using ClearSpam.Domain.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ClearSpam.Application.Rules.Commands
{
    public class DeleteRuleCommandHandler : DeleteEntityCommandHandler<Rule>,
                                             IRequestHandler<DeleteRuleCommand>
    {
        public DeleteRuleCommandHandler(IRepository repository) :
            base(repository)
        { }

        public async Task<Unit> Handle(DeleteRuleCommand request, CancellationToken cancellationToken)
        {
            Handle(request);

            return Unit.Value;
        }
    }
}