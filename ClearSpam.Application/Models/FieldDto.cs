using ClearSpam.Application.Interfaces;

namespace ClearSpam.Application.Models
{
    public class FieldDto : IEntityDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
