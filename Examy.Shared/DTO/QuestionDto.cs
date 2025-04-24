using System.ComponentModel.DataAnnotations;

namespace Examy.Shared.DTO
{
    public class QuestionDto
    {
        public int Id { get; set; }

        [Required, MaxLength(250)]
        public string Text { get; set; }
        
        public List<OptionDto> Options { get; set; } = [];

    }
}
