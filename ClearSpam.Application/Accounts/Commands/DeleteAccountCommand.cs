using ClearSpam.Application.BaseMediator.Commands;
using MediatR;

namespace ClearSpam.Application.Accounts.Commands
{
    public class DeleteAccountCommand : DeleteEntityCommand, IRequest
    {
        public DeleteAccountCommand() : base()
        {
        }

        public DeleteAccountCommand(int id) : base(id)
        {
        }
    }
}