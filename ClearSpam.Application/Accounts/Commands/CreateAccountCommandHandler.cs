using AutoMapper;
using ClearSpam.Application.BaseMediator.Commands;
using ClearSpam.Application.Interfaces;
using ClearSpam.Application.Models;
using ClearSpam.Domain.Entities;
using ClearSpam.Domain.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ClearSpam.Application.Accounts.Commands
{
    public class CreateAccountCommandHandler : CreateEntityCommandHandler<Account, AccountDto>,
                                             IRequestHandler<CreateAccountCommand, AccountDto>
    {
        private readonly ICryptography cryptography;

        public CreateAccountCommandHandler(IRepository repository, IMapper mapper, ICryptography cryptography) :
            base(repository, mapper)
        {
            this.cryptography = cryptography;
        }

        public async Task<AccountDto> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            request.Password = cryptography.Encrypt(request.Password);
            return Handle(request);
        }
    }
}