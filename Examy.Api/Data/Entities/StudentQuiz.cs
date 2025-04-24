using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Examy.Api.Data.Entities;

public class StudentQuiz
{
    [Key]
    public int Id { get; set; }

    public int StudentId { get; set; }

    public Guid QuizId { get; set; }

    public DateTime StartedOn { get; set; }

    public DateTime? CompletedOn { get; set; }

    [AllowedValues(
        nameof(StudentQuizStats.Started),
        nameof(StudentQuizStats.Completed),
        nameof(StudentQuizStats.AutoSubmitted),
        nameof(StudentQuizStats.Expired))]
    public string Status { get; set; } = nameof(StudentQuizStats.Started);
    public int Score { get; set; }

    [ForeignKey(nameof(StudentId))]
    public virtual User Student { get; set; }

    [ForeignKey(nameof(QuizId))]
    public virtual Quiz Quiz { get; set; }

    public virtual ICollection<StudentQuizQuestion> StudentQuizQuestion { get; set; } = [];
}