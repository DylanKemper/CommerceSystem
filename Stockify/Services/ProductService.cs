using Stockify.Models;

namespace Stockify.Services
{
    public class ProductService : IProductService
    {
        private readonly List<Product> _products = new();

        public ProductService()
        {
           _products.Add(new Product { Id = 1, Name = "Dylan", Description = "Groot Piel", Price = 0.01m, Quantity = 1 });

        }
        
        public Product Add(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }
            else 
            {
                _products.Add(product);
            }
            return product;
                  
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _products.ToList();
        }

        public Product? GetProductById(int id)
        {
            return _products.FirstOrDefault(p => p.Id == id);
            //Product? sampleproduct = _products.FirstOrDefault(p => p.Id == id);
            //sampleproduct.Id = 69;
            //return sampleproduct;
        }

        public bool UpdateProduct(Product product)
        {
            var existingProduct = _products.FirstOrDefault(p => p.Id == product.Id);
            if (existingProduct == null)
            {
                return false;
            }
            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            existingProduct.Quantity = product.Quantity;
            return true;
        }

        public bool DeleteProduct(int id)
        {
            var existing = GetProductById(id);
            if (existing is null) return false;

            _products.Remove(existing);
            return true;
        }

        //public void SayHi() 
        //{ 
        //Console.WriteLine("Hi from ProductService!");
        //}

        //Task<Product?> IProductService.UpdateProductAsync(int id, Product product)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
