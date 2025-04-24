using Examy.Shared.DTO;
using Examy.Shared;
using Microsoft.EntityFrameworkCore;
using Examy.Api.Data;

namespace Examy.Api.Services;

public class AdminService
{
       
    private readonly IDbContextFactory<QuizContext> _contextFactory;

    public AdminService(IDbContextFactory<QuizContext> contextFactory )
    {
           
        _contextFactory = contextFactory;
    }



    public async Task<AdminHomeDateDto> GetHomeDataAsync()
    {
        var totalCategoriesTask = _contextFactory.CreateDbContext().Categories.CountAsync();
        var totalStudentsTask = _contextFactory.CreateDbContext().Users
            .Where(u => u.Role == nameof(UserRole.Student))
            .CountAsync();
        var approvedStudentsTask = _contextFactory.CreateDbContext().Users
            .Where(u => u.IsApproved &&  u.Role == nameof(UserRole.Student))
            .CountAsync();
        var totalQuizesTask = _contextFactory.CreateDbContext().Quizzes.CountAsync();
        var activeQuizesTask = _contextFactory.CreateDbContext().Quizzes.Where(q => q.IsActive).CountAsync();

        var totalCategories = await totalCategoriesTask;
        var totalStudents = await totalStudentsTask;
        var approvedStudents = await approvedStudentsTask;
        var totalQuizes = await totalQuizesTask;
        var activeQuizes = await activeQuizesTask;


        return new AdminHomeDateDto(
            totalCategories,
            totalStudents,
            approvedStudents,
            totalQuizes,
            activeQuizes
        );
    }

    public async Task<PagedResult<UserDto>> GetUsersAsync(UserApprovedFilter filter, int startIndex, int pageSize)
    {
        using var context = _contextFactory.CreateDbContext();
        var query = context.Users.AsQueryable();
        if (filter != UserApprovedFilter.All)
        {
            if (filter == UserApprovedFilter.ApprovedOnly)
            {
                query = query.Where(u => u.IsApproved);
            }
            else
            {
                query = query.Where(u => !u.IsApproved);
            }
        }
        var total = await query.CountAsync();
        var users = await query.OrderByDescending(u => u.Id)
            .Skip(startIndex)
            .Take(pageSize)
            .Select(u => new UserDto(u.Id, u.Name, u.Email, u.Phone, u.IsApproved))
            .ToArrayAsync();

        foreach (var user in users)
        {
            Console.WriteLine($"User ID: {user.Id}, Name: {user.Name}");
        }

        return new PagedResult<UserDto>(users, total);
    }

    public async Task ToggleUserApprovedStatus(int userId)
    {
        using var context = _contextFactory.CreateDbContext();
        var dbUser = await context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (dbUser != null)
        {
            dbUser.IsApproved = !dbUser.IsApproved;
            await context.SaveChangesAsync();
            return;
        }
    }
}