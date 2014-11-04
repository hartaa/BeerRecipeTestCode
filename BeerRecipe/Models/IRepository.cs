using System.Collections.Generic;
using System.Linq;

namespace BeerRecipe.Models
{
    public interface IRepository<T>
    {
        T Save(T document);
        void SaveRange(IEnumerable<T> documents);
        T Update(T document);        
        void Remove(string id);
        IQueryable<T> AsQueryable();
        T GetById(string id);
    }
}
