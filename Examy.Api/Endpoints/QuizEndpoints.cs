using Examy.Api.Services;
using Examy.Shared.DTO;

namespace Examy.Api.Endpoints;

public static class QuizEndpoints
{
    public static IEndpointRouteBuilder MapQuizEndpoints(this IEndpointRouteBuilder app)
    {
        var quizGroup = app.MapGroup("/api/quizes").RequireAuthorization();

        quizGroup.MapPost("", async ( QuizSaveDto dto, QuizService service) =>
        {
            if(dto.Questions.Count == 0)
            {
                return Results.BadRequest("Please provide Questions");
            }

            if(dto.Questions.Count != dto.TotalQuestions)
            {
                return Results.BadRequest("Total Questions count does not match with provided questions");
            }
            return Results.Ok(await service.SaveQuizAsync(dto));
        });

        quizGroup.MapGet("", async (QuizService service) 
            => Results.Ok(await service.GetQuizesAsync()));

        quizGroup.MapGet("{quizId:guid}/questions", async (Guid quizId, QuizService service) =>
        {
            return Results.Ok(await service.GetQuizQuestions(quizId));
        });

        quizGroup.MapGet("{quizId:guid}", async (Guid quizId, QuizService service) =>
        {
            return Results.Ok(await service.GetQuizToEditAsync(quizId));
        });

        quizGroup.MapDelete("{quizId:guid}", async (Guid quizId, QuizService service) =>
        {
            return Results.Ok(await service.RemoveQuizAsync(quizId));
        });

        return app;
    }

}