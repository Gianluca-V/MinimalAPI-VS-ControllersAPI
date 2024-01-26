using FullRESTAPI.Models;

namespace FullRESTAPI.Interfaces
{
    public interface IProductRepository
    {
        ICollection<Product> GetProducts();
        Product GetProduct(int Id);
        bool CreateProduct(Product Product);
        bool UpdateProduct(Product Product);
        bool ProductExists(int Id);
        bool DeleteProduct(int Id);
    }
}
