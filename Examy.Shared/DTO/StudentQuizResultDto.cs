namespace Examy.Shared.DTO;

public sealed record StudentQuizResultDto(
    int TotalQuestions,
    int CorrectAnswers,
    int IncorrectAnswers,
    double FinalScore);

