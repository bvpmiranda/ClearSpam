using ClearSpam.Domain.Interfaces;

namespace ClearSpam.Domain.Entities
{
    public class Field : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
