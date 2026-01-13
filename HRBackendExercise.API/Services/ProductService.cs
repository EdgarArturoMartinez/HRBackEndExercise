using HRBackendExercise.API.Models;
using HRBackendExercise.API.Abstractions;

namespace HRBackendExercise.API.Services
{
    public class ProductService : IProductService
    {
        private readonly List<Product> _products = new List<Product>();
        private int _nextId = 1;

        public Product Create(Product entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (string.IsNullOrWhiteSpace(entity.SKU))
                throw new ArgumentException("SKU cannot be null or empty.", nameof(entity.SKU));

            if (entity.Price <= 0)
                throw new ArgumentException("Price must be greater than 0.", nameof(entity.Price));

            entity.Id = _nextId++;
            _products.Add(entity);
            return entity;
        }

        public Product? GetById(int id)
        {
            return _products.FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<Product> GetAll()
        {
            return _products;
        }

        public void Update(Product entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (string.IsNullOrWhiteSpace(entity.SKU))
                throw new ArgumentException("SKU cannot be null or empty.", nameof(entity.SKU));

            if (entity.Price <= 0)
                throw new ArgumentException("Price must be greater than 0.", nameof(entity.Price));

            var existingProduct = GetById(entity.Id);
            if (existingProduct == null)
                throw new InvalidOperationException($"Product with ID {entity.Id} not found.");

            existingProduct.Description = entity.Description;
            existingProduct.SKU = entity.SKU;
            existingProduct.Price = entity.Price;
        }

        public void Delete(Product entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var existingProduct = GetById(entity.Id);
            if (existingProduct != null)
            {
                _products.Remove(existingProduct);
            }
        }
    }
}
