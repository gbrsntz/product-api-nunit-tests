# Products Solution

Projeto acadêmico em C#/.NET com:

- API REST para CRUD de produtos
- interface web simples para demonstração
- testes unitários e de integração com NUnit

## Estrutura

```text
ProductsSolution.sln
global.json
README.md
ProductsApi/
  ProductsApi.csproj
  Program.cs
  Models/
    Product.cs
  Repositories/
    IProductRepository.cs
    ProductRepository.cs
  Services/
    IProductService.cs
    ProductService.cs
  Controllers/
    ProductsController.cs
  wwwroot/
    index.html
    styles.css
    app.js
ProductsApi.Tests/
  ProductsApi.Tests.csproj
  CustomWebApplicationFactory.cs
  Unit/
    ProductServiceTests.cs
  Integration/
    ProductApiIntegrationTests.cs
```

## Como executar

1. Restaurar os pacotes:

```bash
dotnet restore ProductsSolution.sln
```

2. Executar a API:

```bash
dotnet run --project ProductsApi
```

3. Executar os testes:

```bash
dotnet test ProductsApi.Tests
```

## Endpoints

- `GET /products`
- `GET /products/{id}`
- `POST /products`
- `PUT /products/{id}`
- `DELETE /products/{id}`

## Função de cada camada

- `Model`: representa a entidade `Product` com `Id`, `Name` e `Price`.
- `Repository`: faz o armazenamento em memória e centraliza operações de acesso aos dados.
- `Service`: aplica as regras de negócio e valida os dados antes de salvar ou atualizar.
- `Controller`: expõe os endpoints HTTP e converte resultados em respostas como `200`, `201`, `400`, `404` e `204`.

## O que os testes validam

- `ProductServiceTests`: validam as regras da camada de serviço, como cadastro válido, nome obrigatório, preço maior que zero, busca por id, atualização e exclusão.
- `ProductApiIntegrationTests`: validam o comportamento real da API com `HttpClient`, cobrindo sucesso no `GET`, criação no `POST`, atualização no `PUT` e exclusão no `DELETE`.

## Como apresentar em sala

1. Explique rapidamente a arquitetura em camadas e mostre que o repositório é em memória para simplificar o trabalho.
2. Abra a interface web, cadastre um produto, edite e exclua para demonstrar o CRUD funcionando.
3. Mostre os endpoints no controller e diga que as regras ficam no service.
4. Finalize executando `dotnet test` para provar os testes unitários e de integração.
