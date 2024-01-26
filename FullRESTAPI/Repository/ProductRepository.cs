using FullRESTAPI.Interfaces;
using FullRESTAPI.Models;

namespace FullRESTAPI.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductsApiContext context;
        public ProductRepository(ProductsApiContext context)
        {
            this.context = context;
        }

        public ICollection<Product> GetProducts()
        {
            return this.context.Products.OrderBy(p => p.Id).ToList();
        }

        public Product GetProduct(int Id)
        {
            return context.Products.Where(p => p.Id == Id).FirstOrDefault();
        }

        public bool CreateProduct(Product Product)
        {
            context.Add(Product);
            return Save();
        }

        public bool UpdateProduct(Product Product)
        {
            context.Products.Update(Product);
            return Save();
        }

        public bool DeleteProduct(int Id)
        {
            context.Products.Remove(GetProduct(Id));
            return Save();
        }

        public bool ProductExists(int Id)
        {
            if (GetProduct(Id) is not null) return true;
            return false;
        }

        public bool Save()
        {
            var saved = context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
