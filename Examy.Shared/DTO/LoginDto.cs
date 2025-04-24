using System.ComponentModel.DataAnnotations;

namespace Examy.Shared.DTO;

public class LoginDto
{
    [Required, EmailAddress, DataType(DataType.EmailAddress)]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
}