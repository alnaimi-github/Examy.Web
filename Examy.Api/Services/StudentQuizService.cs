using Examy.Api.Data;
using Examy.Api.Data.Entities;
using Examy.Shared.DTO;
using Microsoft.EntityFrameworkCore;

namespace Examy.Api.Services;

public class StudentQuizService
{
    private readonly QuizContext _context;
    public StudentQuizService(QuizContext context)
    {
        _context = context;
    }

    public async Task<QuizListDto[]> GetActiveQuizesAsync( int categoryId)
    {
        var query = _context.Quizzes.Where(q => q.IsActive);
        if (categoryId > 0)
        {
            query = query.Where(q => q.CategoryId == categoryId);
        } 
        var quizes = await _context.Quizzes
            .Where(q => q.CategoryId == categoryId && q.IsActive)
            .Select(q => new QuizListDto
            {
                Id = q.Id,
                Name = q.Name,
                TimeInMinutes = q.TimeInMinutes,
                TotalQuestions = q.Questions.Count,
                CategoryId = q.CategoryId,
                CategoryName = q.Category.Name
            })
            .ToArrayAsync();
        return quizes;
    }
    
    public async Task<QuizApiResponse<int>> StartQuizAsync(int studentId, Guid quizId)
    {
        try
        {
            var studentQuiz = new StudentQuiz
            {
                StudentId = studentId,
                QuizId = quizId,
                Status = nameof(StudentQuizStats.Started),
                StartedOn = DateTime.Now,
            };
            _context.StudentQuizzes.Add(studentQuiz);
            await _context.SaveChangesAsync();

            return QuizApiResponse<int>.Success(studentQuiz.Id);
        }
        catch (Exception ex)
        {
            return QuizApiResponse<int>.Fail(ex.Message);
        }
    }

    public async Task<QuizApiResponse<QuestionDto?>> GetNextQuestionForQuizAsync(int studentQuizId, int studentId)
    {
        var studentQuiz = await _context.StudentQuizzes
            .Include(s => s.StudentQuizQuestion)
            .FirstOrDefaultAsync(s => s.Id == studentQuizId);

        if (studentQuiz == null)
        {
            return QuizApiResponse<QuestionDto?>.Fail("Quiz does not exist");
        }

        if (studentQuiz.StudentId != studentId)
        {
            return QuizApiResponse<QuestionDto?>.Fail("Invalid request");
        }

        var questionsServed = studentQuiz.StudentQuizQuestion
            .Select(s => s.QuestionId)
            .ToArray();

        var nextQuestion = await _context.Questions
            .Where(q => q.QuizId == studentQuiz.QuizId)
            .Where(q => !questionsServed.Contains(q.Id))
            .OrderBy(q => Guid.NewGuid())
            .Select(q => new QuestionDto
            {
                Id = q.Id,
                Text = q.Text,
                Options = q.Options.Select(o => new OptionDto
                {
                    Id = o.Id,
                    Text = o.Text
                }).ToList()
            })
            .Take(1).FirstOrDefaultAsync();

        if (nextQuestion == null)
        {
            return QuizApiResponse<QuestionDto?>.Fail("No more questions for this quiz");
        }

        try
        {
            var studentQuizQuestion = new StudentQuizQuestion
            {
                StudentQuizId = studentQuizId,
                QuestionId = nextQuestion.Id
            };
            _context.StudentQuestions.Add(studentQuizQuestion);
            await _context.SaveChangesAsync();
            return QuizApiResponse<QuestionDto?>.Success(nextQuestion);
        }
        catch (Exception ex)
        {
            return QuizApiResponse<QuestionDto?>.Fail(ex.Message);
        }
    }


    public async Task<QuizApiResponse> SaveQuestionResponseAsync(StudentQuizQuestionResponseDto dto, int studentId)
    {
        var studentQuiz = await _context.StudentQuizzes
            .AsTracking()
            .FirstOrDefaultAsync(s => s.Id == dto.StudentQuizId);
        if (studentQuiz == null)
        {
            return QuizApiResponse.Fail("Quiz does not exist");
        }
        if (studentQuiz.StudentId != studentId)
        {
            return QuizApiResponse.Fail("Invalid request");
        }
        var isSelectedOptionCorrect = await _context.Options
            .Where(o => o.QuestionId == dto.QuestionId && o.Id == dto.OptionId)
            .Select(o => o.IsCorrect)
            .FirstOrDefaultAsync();

        if (isSelectedOptionCorrect)
        {
            studentQuiz.Score++;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return QuizApiResponse.Fail(ex.Message);
            }
        }
        return QuizApiResponse.Success;
    }

    public async Task<QuizApiResponse<StudentQuizResultDto>> SubmitQuizAsync(int studentQuizId, int studentId)
        => await CompleteQuizAsync(studentQuizId, DateTime.Now, nameof(StudentQuizStats.Completed), studentId);
        

    public async Task<QuizApiResponse<StudentQuizResultDto>> ExitQuizAsync(int studentQuizId, int studentId)
        => await CompleteQuizAsync(studentQuizId, null, nameof(StudentQuizStats.Expired), studentId);

    public async Task<QuizApiResponse<StudentQuizResultDto>> AutoSubmitQuizAsync(int studentQuizId, int studentId)
        => await CompleteQuizAsync(studentQuizId, DateTime.Now, nameof(StudentQuizStats.AutoSubmitted), studentId);

    private async Task<QuizApiResponse<StudentQuizResultDto>> CompleteQuizAsync(int studentQuizId, DateTime? completeOn, string status, int studentId)
    {
        var studentQuiz = await _context.StudentQuizzes
            .Include(sq => sq.Quiz)
            .AsTracking()
            .FirstOrDefaultAsync(s => s.Id == studentQuizId);

        if (studentQuiz == null)
        {
            return QuizApiResponse<StudentQuizResultDto>.Fail("Quiz does not exist");
        }

        if (studentQuiz.StudentId != studentId)
        {
            return QuizApiResponse<StudentQuizResultDto>.Fail("Invalid request");
        }

        if (studentQuiz.CompletedOn.HasValue || studentQuiz.Status == nameof(StudentQuizStats.Expired))
        {
            return QuizApiResponse<StudentQuizResultDto>.Fail("Quiz already submitted");
        }

        try
        {
            studentQuiz.Status = status;
            studentQuiz.CompletedOn = completeOn;

            // Calculate the total score
            var totalQuestions = await _context.Questions
                .CountAsync(q => q.QuizId == studentQuiz.QuizId);

            var correctAnswers = studentQuiz.Score;
            var incorrectAnswers = totalQuestions - correctAnswers;
            var finalScore = totalQuestions > 0 ? ((double)correctAnswers / totalQuestions) * 100 : 0;

            await _context.SaveChangesAsync();

            var result = new StudentQuizResultDto(totalQuestions,
                correctAnswers,
                incorrectAnswers,
                finalScore);
               

            return QuizApiResponse<StudentQuizResultDto>.Success(result);
        }
        catch (Exception ex)
        {
            return QuizApiResponse<StudentQuizResultDto>.Fail(ex.Message);
        }
    }



    public async Task<PagedResult<StudentQuizDto>> GetStudentQuizAsync(int  studentId,int startIndex, int pageSize )
    {
        var query = _context.StudentQuizzes.Where(
            p=> p.StudentId == studentId);

        var count = await query.CountAsync();

        var quizes = await query.OrderByDescending(q => q.StartedOn)
            .Skip(startIndex)
            .Take(pageSize)
            .Select(q => new StudentQuizDto
            {
                Id = q.Id,
                QuizId = q.QuizId,
                QuizName = q.Quiz.Name,
                CategoryName = q.Quiz.Category.Name,
                StartedOn = q.StartedOn,
                CompletedOn = q.CompletedOn,
                Status = q.Status,
                Score = q.Score
            })
            .ToArrayAsync();

        return new PagedResult<StudentQuizDto>(quizes, count);
    }

}