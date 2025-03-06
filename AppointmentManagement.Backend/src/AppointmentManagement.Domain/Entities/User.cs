using System.ComponentModel.DataAnnotations;

namespace AppointmentManagement.Domain.Entities
{
    public class User : EntityBase
    {
        [Required, MaxLength(100)]
        public string Username { get; set; } = string.Empty;

        [Required, MaxLength(255), EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;

        public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
    }
}
