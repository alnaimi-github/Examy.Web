namespace Examy.Api.Data.Entities
{
    public class StudentQuizQuestion
    {

        public int StudentQuizId { get; set; }

        public int QuestionId { get; set; }

        public virtual StudentQuiz StudentQuiz { get; set; }

        public virtual Question Question { get; set; }
    }
}
