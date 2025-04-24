using System.ComponentModel.DataAnnotations;

namespace Examy.Shared.DTO;

public class QuizSaveDto
{
    public Guid Id { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; }

    [Range(1, int.MaxValue, ErrorMessage ="Category is required")]
    public int CategoryId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Please provide valid Number of questions")]
    public int TotalQuestions { get; set; }

    [Range(1, 120, ErrorMessage = "Please provide valid time in minutes")]
    public int TimeInMinutes { get; set; }

    public bool IsActive { get; set; }

    public List<QuestionDto> Questions { get; set; } = [];

    public string Validate()
    {
        if (TotalQuestions != Questions.Count)
        {
            return "Number of questions does not match with Total Questions";
        }
        if (Questions.Any(q => string.IsNullOrWhiteSpace(q.Text)))
        {
               
            return "Questions text is required";
        }
        if (Questions.Any(q => q.Options.Count < 2))
        {
                
            return "At least 2 Options are required for questions";
        }
        if (Questions.Any(q => !q.Options.Any(o => o.IsCorrect)))
        {

            return "All options should have correct answer marked";
        }
        return null;
    }
}