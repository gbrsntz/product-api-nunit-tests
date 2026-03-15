using ProductsApi.Models;

namespace ProductsApi.Services;

public interface IProductService
{
    List<Product> GetAll();
    Product? GetById(int id);
    Product Add(Product product);
    Product? Update(int id, Product product);
    bool Delete(int id);
}
