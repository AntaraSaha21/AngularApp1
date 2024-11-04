using AngularApp1.Server.Models;
using Microsoft.AspNetCore.Mvc;

namespace AngularApp1.Server.Service
{
    public interface IProductsService
    {
        Task<IEnumerable<GetProducts>> GetAll();
        Task createProduct(Products products);
        Task<bool> deleteProduct(int productId);
        Task<IEnumerable<GetProducts>> GetProductById(int productId);
        Task updateProduct(Products products,int productId);
        Task<bool> productExists(int productId);
    }
}
