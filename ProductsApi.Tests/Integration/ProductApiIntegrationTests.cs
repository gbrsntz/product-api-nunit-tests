using System.Net;
using System.Net.Http.Json;
using NUnit.Framework;
using ProductsApi.Models;

namespace ProductsApi.Tests.Integration;

[TestFixture]
public class ProductApiIntegrationTests
{
    private CustomWebApplicationFactory _factory = null!;
    private HttpClient _client = null!;

    [SetUp]
    public void SetUp()
    {
        _factory = new CustomWebApplicationFactory();
        _factory.ResetData();
        _client = _factory.CreateClient();
    }

    [TearDown]
    public void TearDown()
    {
        _client.Dispose();
        _factory.Dispose();
    }

    [Test]
    public async Task GetProducts_ShouldReturnSuccess()
    {
        var response = await _client.GetAsync("/products");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task PostProduct_ShouldCreateProduct()
    {
        var response = await _client.PostAsJsonAsync("/products", new Product
        {
            Name = "Headset",
            Price = 199.90m
        });

        var createdProduct = await response.Content.ReadFromJsonAsync<Product>();

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        Assert.That(createdProduct, Is.Not.Null);
        Assert.That(createdProduct!.Id, Is.GreaterThan(0));
        Assert.That(createdProduct.Name, Is.EqualTo("Headset"));
    }

    [Test]
    public async Task PutProduct_ShouldUpdateExistingProduct()
    {
        var createResponse = await _client.PostAsJsonAsync("/products", new Product
        {
            Name = "Mesa Digitalizadora",
            Price = 580m
        });

        var createdProduct = await createResponse.Content.ReadFromJsonAsync<Product>();

        var updateResponse = await _client.PutAsJsonAsync($"/products/{createdProduct!.Id}", new Product
        {
            Name = "Mesa Digitalizadora Pro",
            Price = 790m
        });

        var updatedProduct = await updateResponse.Content.ReadFromJsonAsync<Product>();

        Assert.That(updateResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(updatedProduct, Is.Not.Null);
        Assert.That(updatedProduct!.Name, Is.EqualTo("Mesa Digitalizadora Pro"));
        Assert.That(updatedProduct.Price, Is.EqualTo(790m));
    }

    [Test]
    public async Task DeleteProduct_ShouldReturnNoContent()
    {
        var createResponse = await _client.PostAsJsonAsync("/products", new Product
        {
            Name = "Hub USB",
            Price = 89.90m
        });

        var createdProduct = await createResponse.Content.ReadFromJsonAsync<Product>();
        var deleteResponse = await _client.DeleteAsync($"/products/{createdProduct!.Id}");

        Assert.That(deleteResponse.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
    }
}
