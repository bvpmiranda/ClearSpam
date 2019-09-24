using ClearSpam.Application.BaseMediator.Commands;
using ClearSpam.Domain.Entities;
using ClearSpam.Domain.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ClearSpam.Application.Accounts.Commands
{
    public class DeleteAccountCommandHandler : DeleteEntityCommandHandler<Account>,
                                             IRequestHandler<DeleteAccountCommand>
    {
        public DeleteAccountCommandHandler(IRepository repository) :
            base(repository)
        { }

        public async Task<Unit> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
        {
            Handle(request);

            return Unit.Value;
        }
    }
}