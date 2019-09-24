using AutoMapper;
using ClearSpam.Application.Exceptions;
using ClearSpam.Application.Interfaces;
using ClearSpam.Domain.Interfaces;

namespace ClearSpam.Application.BaseMediator.Commands
{
    public abstract class UpdateEntityCommandHandler<TEntity, TEntityDto>
        where TEntity : class
        where TEntityDto : class
    {
        protected readonly IMapper Mapper;
        protected readonly IRepository Repository;

        public UpdateEntityCommandHandler(IRepository repository, IMapper mapper)
        {
            Repository = repository;
            Mapper = mapper;
        }

        public TEntityDto Handle(IEntityDto request)
        {
            var entity = Repository.Get<TEntity>(request.Id);

            if (entity == null)
                throw new NotFoundException(nameof(entity), request.Id);

            Mapper.Map(request, entity);

            Repository.SaveChanges();

            return Mapper.Map<TEntityDto>(entity);
        }
    }
}