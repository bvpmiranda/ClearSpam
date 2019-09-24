using AutoMapper;
using ClearSpam.Application.BaseMediator.Queries;
using ClearSpam.Application.Models;
using ClearSpam.Domain.Entities;
using ClearSpam.Domain.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ClearSpam.Application.Fields.Queries
{
    public class GetFieldsQueryHandler : GetEntitiesQueryHandler<Field, FieldDto>, IRequestHandler<GetFieldsQuery, IEnumerable<FieldDto>>
    {
        public GetFieldsQueryHandler(IRepository repository, IMapper mapper) : base(repository, mapper)
        {
        }

        public Task<IEnumerable<FieldDto>> Handle(GetFieldsQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(Handle());
        }
    }
}
