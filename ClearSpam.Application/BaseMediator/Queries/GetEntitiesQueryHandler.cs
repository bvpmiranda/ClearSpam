using AutoMapper;
using ClearSpam.Common;
using ClearSpam.Domain.Interfaces;
using System.Collections.Generic;

namespace ClearSpam.Application.BaseMediator.Queries
{
    public abstract class GetEntitiesQueryHandler<TEntity, TDto>
        where TEntity : class
        where TDto : class
    {
        protected readonly IMapper Mapper;
        protected readonly IRepository Repository;

        public GetEntitiesQueryHandler(IRepository repository, IMapper mapper)
        {
            Repository = repository;
            Mapper = mapper;
        }

        public IEnumerable<TDto> Handle()
        {
            var entities = Repository.Get<TEntity>();
            var result = Mapper.MapList<TEntity, TDto>(entities);

            return result;
        }
    }
}