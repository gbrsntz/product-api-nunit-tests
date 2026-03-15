using NUnit.Framework;
using ProductsApi.Models;
using ProductsApi.Repositories;
using ProductsApi.Services;

namespace ProductsApi.Tests.Unit;

[TestFixture]
public class ProductServiceTests
{
    private IProductRepository _repository = null!;
    private IProductService _service = null!;

    [SetUp]
    public void SetUp()
    {
        _repository = new ProductRepository();
        _service = new ProductService(_repository);
    }

    [Test]
    public void AddProduct_WithValidData_ShouldAddSuccessfully()
    {
        var product = new Product { Name = "Mouse", Price = 50m };

        var createdProduct = _service.Add(product);
        var allProducts = _service.GetAll();

        Assert.That(createdProduct.Id, Is.GreaterThan(0));
        Assert.That(createdProduct.Name, Is.EqualTo("Mouse"));
        Assert.That(createdProduct.Price, Is.EqualTo(50m));
        Assert.That(allProducts, Has.Count.EqualTo(1));
    }

    [Test]
    public void AddProduct_WithEmptyName_ShouldThrowException()
    {
        var product = new Product { Name = "   ", Price = 50m };

        var action = () => _service.Add(product);

        Assert.That(action, Throws.ArgumentException.With.Message.EqualTo("Product name is required."));
    }

    [Test]
    public void AddProduct_WithInvalidPrice_ShouldThrowException()
    {
        var product = new Product { Name = "Teclado", Price = 0m };

        var action = () => _service.Add(product);

        Assert.That(action, Throws.ArgumentException.With.Message.EqualTo("Product price must be greater than zero."));
    }

    [Test]
    public void GetById_WithExistingId_ShouldReturnProduct()
    {
        var createdProduct = _service.Add(new Product { Name = "Monitor", Price = 899.90m });

        var foundProduct = _service.GetById(createdProduct.Id);

        Assert.That(foundProduct, Is.Not.Null);
        Assert.That(foundProduct!.Id, Is.EqualTo(createdProduct.Id));
        Assert.That(foundProduct.Name, Is.EqualTo("Monitor"));
    }

    [Test]
    public void Update_WithValidData_ShouldUpdateProduct()
    {
        var createdProduct = _service.Add(new Product { Name = "Notebook", Price = 3500m });

        var updatedProduct = _service.Update(createdProduct.Id, new Product
        {
            Name = "Notebook Pro",
            Price = 4200m
        });

        Assert.That(updatedProduct, Is.Not.Null);
        Assert.That(updatedProduct!.Name, Is.EqualTo("Notebook Pro"));
        Assert.That(updatedProduct.Price, Is.EqualTo(4200m));
        Assert.That(_service.GetById(createdProduct.Id)!.Name, Is.EqualTo("Notebook Pro"));
    }

    [Test]
    public void Delete_WithExistingId_ShouldRemoveProduct()
    {
        var createdProduct = _service.Add(new Product { Name = "Webcam", Price = 210m });

        var deleted = _service.Delete(createdProduct.Id);
        var foundProduct = _service.GetById(createdProduct.Id);

        Assert.That(deleted, Is.True);
        Assert.That(foundProduct, Is.Null);
        Assert.That(_service.GetAll(), Is.Empty);
    }
}
