using MediatR;

namespace ClearSpam.Application.ClearSpam.Commands
{
    public class ClearSpamCommand : IRequest
    {
        public int? Id { get; set; }

        public ClearSpamCommand()
        {

        }

        public ClearSpamCommand(int? id)
        {
            Id = id;
        }
    }
}
