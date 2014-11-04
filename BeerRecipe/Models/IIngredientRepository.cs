using System.Linq;

namespace BeerRecipe.Models
{
    interface IIngredientRepository<T> : IRepository<T>
    {
        IQueryable<T> FindIngredients(string query);
    }
}
