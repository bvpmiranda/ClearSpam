using AutoMapper;
using ClearSpam.Application.Exceptions;
using ClearSpam.Application.Interfaces;
using ClearSpam.Domain.Interfaces;
using System;

namespace ClearSpam.Application.BaseMediator.Queries
{
    public abstract class GetEntityQueryHandler<TEntity, TDto>
        where TEntity : class
        where TDto : class
    {
        protected readonly IMapper Mapper;
        protected readonly IRepository Repository;

        public GetEntityQueryHandler(IRepository repository, IMapper mapper)
        {
            Repository = repository;
            Mapper = mapper;
        }

        public TDto Handle(IEntityDto request)
        {
            if (request.Id <= 0)
                throw new ArgumentOutOfRangeException(nameof(request.Id));

            var entity = Repository.Get<TEntity>(request.Id);

            if (entity == null)
                throw new NotFoundException(nameof(TEntity), request.Id);

            var result = Mapper.Map<TDto>(entity);
            return result;
        }
    }
}