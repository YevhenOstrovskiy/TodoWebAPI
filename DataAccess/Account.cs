using System.ComponentModel.DataAnnotations;

namespace DataAccess
{
    public class Account
    {
        public Guid Id { get; set; }
        
        [Required]
        public string UserName { get; set; }
        
        [Required]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public ICollection<Todo> Todos { get; set; } = new List<Todo>();
    }
}
