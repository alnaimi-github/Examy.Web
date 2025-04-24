using Examy.Shared.DTO;
using Refit;

namespace Examy.Web.Apis
{
    [Headers("Authorization: Bearer ")]
    public interface ICategoryApi
    {
        [Post("/api/categories")]
        Task<QuizApiResponse> CreatedCategoryAsync(CategoryDto categoryDto);

        [Get("/api/categories")]
        Task<CategoryDto[]> GetAllCategoriesAsync();

        [Get("/api/categories/{id}")]
        Task<QuizApiResponse> GetCategoryByIdAsync(int id);

        [Put("/api/categories/{id}")]
        Task<QuizApiResponse> UpdateCategoriesAsync(
            int id, CategoryDto categoryDto);

        [Delete("/api/categories/{id}")]
        Task<QuizApiResponse> DeleteCategoryAsync(int id);

    }
}
