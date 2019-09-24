using ClearSpam.Application.BaseMediator.Commands;
using MediatR;

namespace ClearSpam.Application.Rules.Commands
{
    public class DeleteRuleCommand : DeleteEntityCommand, IRequest
    {
        public DeleteRuleCommand(int id) : base(id)
        {
        }
    }
}