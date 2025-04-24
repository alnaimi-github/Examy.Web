using Examy.Shared.DTO;
using Refit;

namespace Examy.Web.Apis;

[Headers("Authorization: Bearer")]
public interface IQuizApi
{
    [Delete("/api/quizes/{quizId}")]
    Task<QuizApiResponse> DeleteQuizAsync(Guid quizId);
    [Post("/api/quizes")]
    Task<QuizApiResponse> SaveQuizAsync(QuizSaveDto dto);
    [Get("/api/quizes")]
    Task<QuizListDto[]> GetQuizesAsync();

    [Get("/api/quizes/{quizId}/questions")]
    Task<QuestionDto[]> GetQuizQuestionsAsync(Guid quizId);

    [Get("/api/quizes/{quizId}")]
    Task<QuizSaveDto?> GetQuizToEditAsync(Guid quizId);
}