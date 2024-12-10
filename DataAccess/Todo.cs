using System.ComponentModel.DataAnnotations;

namespace DataAccess
{
    public class Todo
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string? Name { get; set; }
        public bool IsComplete { get; set; } = false;

        public Guid AccountId { get; set; }

        public Account Account { get; set; } = null!;
    }
}
