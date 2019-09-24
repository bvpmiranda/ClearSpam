using ClearSpam.Application.Interfaces;

namespace ClearSpam.Application.BaseMediator.Commands
{
    public abstract class DeleteEntityCommand : IDeleteEntityCommand
    {
        public int Id { get; set; }

        public DeleteEntityCommand()
        {

        }

        public DeleteEntityCommand(int id)
        {
            Id = id;
        }
    }
}