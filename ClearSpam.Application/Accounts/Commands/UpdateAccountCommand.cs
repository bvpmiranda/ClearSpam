using ClearSpam.Application.Models;
using MediatR;

namespace ClearSpam.Application.Accounts.Commands
{
    public class UpdateAccountCommand : AccountDto, IRequest<AccountDto>
    {
    }
}