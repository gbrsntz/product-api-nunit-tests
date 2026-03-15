using ProductsApi.Models;

namespace ProductsApi.Repositories;

public interface IProductRepository
{
    List<Product> GetAll();
    Product? GetById(int id);
    Product Add(Product product);
    Product Update(Product product);
    bool Delete(int id);
    void Clear();
    void Seed(IEnumerable<Product> products);
}
