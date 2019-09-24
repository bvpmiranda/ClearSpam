using AutoMapper;
using ClearSpam.Application.Interfaces;
using ClearSpam.Domain.Interfaces;

namespace ClearSpam.Application.BaseMediator.Commands
{
    public abstract class CreateEntityCommandHandler<TEntity, TEntityDto>
         where TEntity : class
         where TEntityDto : class
    {
        private readonly IMapper _mapper;
        private readonly IRepository _repository;

        public CreateEntityCommandHandler(IRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public TEntityDto Handle(IEntityDto request)
        {
            request.Id = 0;

            var entity = _mapper.Map<TEntity>(request);
            _repository.Add(entity);

            _repository.SaveChanges();

            return _mapper.Map<TEntityDto>(entity);
        }
    }
}