using ProductsApi.Models;
using ProductsApi.Repositories;

namespace ProductsApi.Services;

public class ProductService(IProductRepository repository) : IProductService
{
    private readonly IProductRepository _repository = repository;

    public List<Product> GetAll()
    {
        return _repository.GetAll();
    }

    public Product? GetById(int id)
    {
        return _repository.GetById(id);
    }

    public Product Add(Product product)
    {
        Validate(product);

        return _repository.Add(new Product
        {
            Name = product.Name.Trim(),
            Price = product.Price
        });
    }

    public Product? Update(int id, Product product)
    {
        Validate(product);

        var existingProduct = _repository.GetById(id);
        if (existingProduct is null)
        {
            return null;
        }

        existingProduct.Name = product.Name.Trim();
        existingProduct.Price = product.Price;

        return _repository.Update(existingProduct);
    }

    public bool Delete(int id)
    {
        return _repository.Delete(id);
    }

    private static void Validate(Product product)
    {
        if (string.IsNullOrWhiteSpace(product.Name))
        {
            throw new ArgumentException("Product name is required.");
        }

        if (product.Price <= 0)
        {
            throw new ArgumentException("Product price must be greater than zero.");
        }
    }
}
