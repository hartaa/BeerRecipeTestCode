using System.Linq;
using System.Text.RegularExpressions;

namespace BeerRecipe.Models
{
    public class IngredientRepository : Repository<Ingredient>, IIngredientRepository<Ingredient>
    {
        public IQueryable<Ingredient> FindIngredients(string query)
        {
            var pattern = new Regex(string.Format(".*{0}.*", query), RegexOptions.IgnoreCase);
            return AsQueryable().Where(i => pattern.IsMatch(i.Name));
        }
    }
}