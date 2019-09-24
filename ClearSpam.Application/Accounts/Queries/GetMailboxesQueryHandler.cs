using AutoMapper;
using ClearSpam.Application.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ClearSpam.Application.Accounts.Queries
{
    public class GetMailboxesQueryHandler : IRequestHandler<GetMailboxesQuery, IEnumerable<string>>
    {
        private readonly IImapService imapservice;
        private readonly IMapper mapper;

        public GetMailboxesQueryHandler(IImapService imapservice, IMapper mapper)
        {
            this.imapservice = imapservice;
            this.mapper = mapper;
        }

        public Task<IEnumerable<string>> Handle(GetMailboxesQuery request, CancellationToken cancellationToken)
        {
            imapservice.Account = request.Account;
            var result = imapservice.GetMailboxesList().OrderBy(x => x).AsEnumerable();

            return Task.FromResult(result);
        }
    }
}
