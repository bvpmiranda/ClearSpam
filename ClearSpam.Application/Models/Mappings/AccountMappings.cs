using AutoMapper;
using ClearSpam.Application.Accounts.Commands;
using ClearSpam.Domain.Entities;

namespace ClearSpam.Application.Models.Mappings
{
    public class AccountMappings : Profile
    {
        public AccountMappings()
        {
            CreateMap<Account, AccountDto>()
                .ReverseMap();

            CreateMap<CreateAccountCommand, AccountDto>()
                .ReverseMap();

            CreateMap<UpdateAccountCommand, AccountDto>()
                .ReverseMap();
        }
    }
}
