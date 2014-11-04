using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using BeerRecipe.Models;

namespace BeerRecipe.Controllers
{
    public class IngredientsController : ApiController
    {
        private readonly IngredientRepository _ingredientRepository;

        IngredientsController()
        {
            //TODO: Use IOC container with constructor injection...say StructureMap or whatever.
            _ingredientRepository = new IngredientRepository();
            var ingredients = new[]
            { 
                new Ingredient { Id = "1", Name = "Barley" }, 
                new Ingredient { Id = "2", Name = "Hops" }, 
                new Ingredient { Id = "3", Name = "Yeast" }
            };
            LoadSampleData(ingredients);
        }

        private void LoadSampleData(IEnumerable<Ingredient> ingredients)
        {
            //Insert initial data into Mongo
            foreach (var ingredient in ingredients)
            {
                _ingredientRepository.Save(ingredient);
            }
        }

        public IEnumerable<Ingredient> GetIngredients()
        {
            //Enumerate it with ToList() so mongo actually generates the query from the expression trees
            //and executes.  Use AsQueryable() and only enumerate prior to serialization out to client.
            //This will increase performance considerably.
            
            //TODO: Add paging paramters
            return _ingredientRepository.AsQueryable().ToList();
        }

        //Search ingredients by id
        public IHttpActionResult GetIngredient(string id)
        {
            var ingredient = _ingredientRepository.GetById(id);
            if (ingredient == null)
            {
                return NotFound();
            }
            return Ok(ingredient);
        }
        
        //Search ingredients by query string.  Simple Regex at the moment.
        //Elastic Search or Solar (Lucene) would be ideal.
        public IEnumerable<Ingredient> GetIngredients(string query)
        {
            //TODO: Add paging paramters
            var ingredients = _ingredientRepository.FindIngredients(query).ToList();
            return ingredients;
        }

        public Ingredient PostIngredient(Ingredient ingredient)
        {
            //Don't save ingredients without a name
            //TODO: Maybe return a bad request here?
            if (ingredient == null || string.IsNullOrEmpty(ingredient.Name)) return null;

            _ingredientRepository.Save(ingredient);
            return ingredient;
        }
    }
}
