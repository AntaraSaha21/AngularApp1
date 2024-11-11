using AngularApp1.Server.Controllers;
using AngularApp1.Server.Models;
using AngularApp1.Server.Service;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularappAPI_Testing
{
    public class ProductsV1ControllerTests
    {
        private ProductsV1Controller _controller;
        private Mock<IProductsService> _mockService;

        [SetUp]
        public void SetUp()
        {
            _mockService = new Mock<IProductsService>();
            _controller = new ProductsV1Controller(_mockService.Object);
        }
        [Test]
        public async Task GetProducts_ReturnsOkResult()
        {
            // Arrange
            var products = new List<GetProducts> { new GetProducts { Id = 1, Name = "Product1", Quantity = 10, EAN = "123456789" } };
            _mockService.Setup(service => service.GetAll()).ReturnsAsync(products);

            // Act
            var result = await _controller.GetProducts();

            // Assert
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult.Value, Is.InstanceOf<IEnumerable<GetProducts>>());
            Assert.That(okResult.Value, Has.Count.EqualTo(1));
        }

        [Test]
        public async Task GetProducts_WithValidId_ReturnsOkResult()
        {
            // Arrange
            var products = new List<GetProducts>
            {
                new GetProducts { Id = 1, Name = "Product1", Quantity = 10, EAN = "123456789" }
            };

            _mockService.Setup(service => service.productExists(1)).ReturnsAsync(true);
            _mockService.Setup(service => service.GetProductById(1)).ReturnsAsync(products);

            // Act
            var result = await _controller.GetProducts(1);

            // Assert
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult.Value, Is.InstanceOf<IEnumerable<GetProducts>>());
            var returnedProducts = okResult.Value as IEnumerable<GetProducts>;
            Assert.IsNotNull(returnedProducts);
            Assert.AreEqual(1, returnedProducts.Count());
        }

        [Test]
        public async Task GetProducts_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            _mockService.Setup(service => service.productExists(1)).ReturnsAsync(false);

            // Act
            var result = await _controller.GetProducts(1);

            // Assert
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task PostProducts_CreatesProduct_ReturnsCreatedAtAction()
        {
            // Arrange
            var product = new Products { Id = 1, Name = "Product1", Quantity = 10, EAN = "123456789" };
            _mockService.Setup(service => service.createProduct(product)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.PostProducts(product);

            // Assert
            Assert.That(result.Result, Is.TypeOf<CreatedAtActionResult>());
            var createdResult = result.Result as CreatedAtActionResult;
            Assert.That(createdResult.RouteValues["version"], Is.EqualTo("1.0"));
        }

        [Test]
        public async Task DeleteProducts_WithValidId_ReturnsNoContent()
        {
            // Arrange
            _mockService.Setup(service => service.productExists(1)).ReturnsAsync(true);
            _mockService.Setup(service => service.deleteProduct(1)).ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteProducts(1);

            // Assert
            Assert.That(result, Is.TypeOf<NoContentResult>());
        }

        [Test]
        public async Task DeleteProducts_WithInvalidId_ReturnsBadRequest()
        {
            // Arrange
            _mockService.Setup(service => service.productExists(1)).ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteProducts(1);

            // Assert
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }
    }
}
