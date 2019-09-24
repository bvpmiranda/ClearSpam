using ClearSpam.Application.Exceptions;
using ClearSpam.Domain.Interfaces;

namespace ClearSpam.Application.BaseMediator.Commands
{
    public abstract class DeleteEntityCommandHandler<TEntity>
       where TEntity : class
    {
        private readonly IRepository _repository;

        public DeleteEntityCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        public void Handle(DeleteEntityCommand request)
        {
            var entity = _repository.Get<TEntity>(request.Id);

            if (entity == null)
                throw new NotFoundException(nameof(entity), request.Id);

            _repository.Remove<TEntity>(entity);
            _repository.SaveChanges();
        }
    }
}