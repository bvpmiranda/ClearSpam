using ClearSpam.Application.Models;
using MediatR;

namespace ClearSpam.Application.Rules.Commands
{
    public class UpdateRuleCommand : RuleDto, IRequest<RuleDto>
    {
    }
}