using System.ComponentModel.DataAnnotations;

namespace Examy.Shared.DTO
{
    public class RegisterDto
    {
        [Required, MaxLength(50)]
        public string Name { get; set; }

        [Required, EmailAddress, DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required,Length(10,15)]
        public string Phone { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
