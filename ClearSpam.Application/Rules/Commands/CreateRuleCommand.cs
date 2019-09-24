using ClearSpam.Application.Models;
using MediatR;

namespace ClearSpam.Application.Rules.Commands
{
    public class CreateRuleCommand : RuleDto, IRequest<RuleDto>
    {
    }
}