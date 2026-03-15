using ProductsApi.Models;

namespace ProductsApi.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly List<Product> _products = [];
    private int _nextId = 1;

    public List<Product> GetAll()
    {
        return _products
            .Select(Clone)
            .OrderBy(product => product.Id)
            .ToList();
    }

    public Product? GetById(int id)
    {
        var product = _products.FirstOrDefault(item => item.Id == id);
        return product is null ? null : Clone(product);
    }

    public Product Add(Product product)
    {
        var storedProduct = Clone(product);
        storedProduct.Id = _nextId++;
        _products.Add(storedProduct);
        return Clone(storedProduct);
    }

    public Product Update(Product product)
    {
        var existingProduct = _products.First(item => item.Id == product.Id);
        existingProduct.Name = product.Name;
        existingProduct.Price = product.Price;
        return Clone(existingProduct);
    }

    public bool Delete(int id)
    {
        var existingProduct = _products.FirstOrDefault(item => item.Id == id);
        if (existingProduct is null)
        {
            return false;
        }

        _products.Remove(existingProduct);
        return true;
    }

    public void Clear()
    {
        _products.Clear();
        _nextId = 1;
    }

    public void Seed(IEnumerable<Product> products)
    {
        Clear();

        foreach (var product in products)
        {
            var storedProduct = Clone(product);

            if (storedProduct.Id <= 0)
            {
                storedProduct.Id = _nextId;
            }

            _products.Add(storedProduct);
            _nextId = Math.Max(_nextId, storedProduct.Id + 1);
        }
    }

    private static Product Clone(Product product)
    {
        return new Product
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price
        };
    }
}
