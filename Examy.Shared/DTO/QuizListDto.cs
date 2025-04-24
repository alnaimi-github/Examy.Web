namespace Examy.Shared.DTO
{
   public class QuizListDto
    {
        public Guid Id { get; set; }

     
        public string Name { get; set; }

        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public int TotalQuestions { get; set; }

        public int TimeInMinutes { get; set; }

        public bool IsActive { get; set; }
    }
}
