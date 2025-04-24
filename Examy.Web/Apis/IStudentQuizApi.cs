using Examy.Shared.DTO;
using Refit;

namespace Examy.Web.Apis
{
    [Headers("Authorization: Bearer")]
    public interface IStudentQuizApi
    {
        [Get("/api/student/available-quizes")]
        Task<QuizListDto[]> GetActiveQuizesAsync(int categoryId);

        [Get("/api/student/my-quizes")]
        Task<PagedResult<StudentQuizDto>> GetStudentQuizAsync(int startIndex, int pageSize);

        [Post("/api/student/quiz/{quizId}/start")]
        Task<QuizApiResponse<int>> StartQuizAsync(Guid quizId);

        [Get("/api/student/quiz/{studentQuizId}/next-question")]
        Task<QuizApiResponse<QuestionDto?>> GetNextQuestionForQuizAsync(int studentQuizId);

        [Post("/api/student/quiz/{studentQuizId}/save-response")]
        Task<QuizApiResponse> SaveQuestionResponseAsync(int studentQuizId, StudentQuizQuestionResponseDto dto);

        [Post("/api/student/quiz/{studentQuizId}/submit")]
        Task<QuizApiResponse<StudentQuizResultDto>> SubmitQuizAsync(int studentQuizId);

        [Post("/api/student/quiz/{studentQuizId}/auto-submit")]
        Task<QuizApiResponse<StudentQuizResultDto>> AutoSubmitQuizAsync(int studentQuizId);

        [Post("/api/student/quiz/{studentQuizId}/exit")]
        Task<QuizApiResponse<StudentQuizResultDto>> ExitQuizAsync(int studentQuizId);
    }
}
