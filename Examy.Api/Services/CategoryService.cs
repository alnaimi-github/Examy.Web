using Examy.Api.Data;
using Examy.Api.Data.Entities;
using Examy.Shared.DTO;
using Microsoft.EntityFrameworkCore;

namespace Examy.Api.Services;

public class CategoryService
{
    private readonly QuizContext _context;
    public CategoryService(QuizContext context)
    {
        _context = context;
    }

    public async Task<QuizApiResponse> CreateCategoryAsync(CategoryDto dto)
    {
        if(await _context.Categories
               .AsNoTracking().AnyAsync
                   (c => c.Name == dto.Name && c.Id != dto.Id))
        {
            return QuizApiResponse.Fail(
                "Category with the same name already exists"
            );
        }

        if (dto.Id == 0)
        {
            var category = new Category

            {
                //id is auto generated
                        
                Name = dto.Name
            };
            _context.Categories.Add(category);
        }
        else
        {
            var dbCategory = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id != dto.Id);
            if (dbCategory == null) 
            {
                     
                return QuizApiResponse.Fail("Category does not exist");
            }
            dbCategory.Name = dto.Name;
            _context.Categories.Update(dbCategory);

        }
        await _context.SaveChangesAsync();
        return QuizApiResponse.Success;
    }

    public async Task<CategoryDto[]> GetCategoriesAsync()
        => await _context.Categories
            .AsNoTracking()
            .Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name
            })
            .ToArrayAsync();

    public async Task<CategoryDto> GetCategoryByIdAsync(int id)
        => await _context.Categories
            .AsNoTracking()
            .Where(c => c.Id == id)
            .Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name
            })
            .FirstOrDefaultAsync();



    public async Task<CategoryDto> UpdateCategoryAsync(int id, CategoryDto dto)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null)
        {
            return null;
        }
        category.Name = dto.Name;
        await _context.SaveChangesAsync();
        return dto;
    }

    public async Task<bool> DeleteCategoryAsync(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null)
        {
            return false;
        }
        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
        return true;
    }


}