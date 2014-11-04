using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using BeerRecipe.Models;

namespace BeerRecipe.Controllers
{

    //TODO: Fully implement recipe CRUD operations
    public class RecipeController : ApiController
    {
        private readonly RecipeRespository _recipeRespository;

        RecipeController()
        {
            //TODO: Use IOC container with constructor injection...say StructureMap or whatever.
            _recipeRespository = new RecipeRespository();
        }

        // GET api/<controller>
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        public IEnumerable<Recipe> GetRecipes()
        {
            //Enumerate it with ToList() so mongo actually generates the query from the expression trees
            //and executes.  Use AsQueryable() and only enumerate prior to serialization out to client.
            //This will increase performance considerably.

            //TODO: Add paging paramters
            return _recipeRespository.AsQueryable().ToList();
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public Recipe Post([FromBody]Recipe recipe)
        {
            //Don't save recipes without a name or ingredients
            //TODO: Maybe return a bad request here?
            if (recipe == null || string.IsNullOrEmpty(recipe.Name)) return null;

            _recipeRespository.Save(recipe);
            return recipe;
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}