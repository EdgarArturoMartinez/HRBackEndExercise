using HRBackendExercise.API.Models;

namespace HRBackendExercise.API.Abstractions
{
    public interface IProductService
    {
        Product Create(Product entity);
        Product? GetById(int id);
        IEnumerable<Product> GetAll();
        void Update(Product entity);
        void Delete(Product entity);
    }
}
