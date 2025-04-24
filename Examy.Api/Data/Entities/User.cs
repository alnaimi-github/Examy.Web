using System.ComponentModel.DataAnnotations;
using Examy.Shared;

namespace Examy.Api.Data.Entities

{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(150)]
        public string Email { get; set; }

        [Length(10,15)]
        public string Phone { get; set; }

        [MaxLength(250)]
        public string PasswordHash { get; set; }

        [MaxLength(50)]
        public string Role { get; set; } = nameof(UserRole.Student);

        public bool IsApproved { get; set; }
    }
}
