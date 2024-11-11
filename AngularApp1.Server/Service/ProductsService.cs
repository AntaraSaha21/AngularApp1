using AngularApp1.Server.Models;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
namespace AngularApp1.Server.Service
{
    public class ProductsService:IProductsService
    {
        private readonly string _connectionString;
        public ProductsService( IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DatabaseConnection");
        }

        public async Task createProduct(Products products)
        {
            string sqlInsert = "INSERT INTO Products (Name, Quantity,EAN) VALUES (@Name, @Quantity,@EAN)";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(sqlInsert, connection);
                command.Parameters.AddWithValue("@Name", products.Name);
                command.Parameters.AddWithValue("@Quantity", products.Quantity);
                command.Parameters.AddWithValue("@EAN", products.EAN);

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
        }
        public async Task<bool> deleteProduct(int productId)
        {
            string sqlDelete = "DELETE FROM Products WHERE ISNULL(Id,0)=@Id";
            List<Products> productsList = new List<Products>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(sqlDelete, connection);
                command.Parameters.AddWithValue("@Id", productId);

                await connection.OpenAsync();
                int roweffected=await command.ExecuteNonQueryAsync();
                if (roweffected > 0) {
                    return true;
                }
                return false;
            }
        }
        public async Task<IEnumerable<GetProducts>> GetProductById(int productId)
        {
            string sqlGetById = "SELECT Name,Quantity,EAN FROM Products WHERE ISNULL(Id,0)=@Id";
            List<GetProducts> productsList = new List<GetProducts>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(sqlGetById, connection);
                command.Parameters.AddWithValue("@Id", productId);

                await connection.OpenAsync();
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var product = new GetProducts
                        {
                            Id = productId,
                            Name = reader["Name"].ToString(),
                            Quantity = (int)reader["Quantity"],
                            EAN = reader["EAN"].ToString()
                        };
                        productsList.Add(product);
                    }
                }
            }
            return productsList;
        }
        public async Task updateProduct(Products products, int productId)
        {
            var update = new List<string>();
            if(products.Name!=null && products.Name != "")
            {
                update.Add("Name = @Name");
            }
            if (products.Quantity >0)
            {
                update.Add("Quantity=@Quantity");
            }
            if (products.EAN != null && products.EAN !="")
            {
                update.Add("EAN=@EAN");
            }
            string sqlUpdate = $"UPDATE Products SET {string.Join(",",update)} WHERE ISNULL(Id,0)=@Id";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(sqlUpdate, connection);
                command.Parameters.AddWithValue("@Id", productId);
                command.Parameters.AddWithValue("@Name", products.Name);
                command.Parameters.AddWithValue("@Quantity", products.Quantity);
                command.Parameters.AddWithValue("@EAN", products.EAN);

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
        }
        public async Task<bool> productExists(int productId) { 
            bool isExists=false;
            string sqlGetById = "SELECT Name,Quantity,EAN FROM Products WHERE ISNULL(Id,0)=@Id";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(sqlGetById, connection);
                command.Parameters.AddWithValue("@Id", productId);

                await connection.OpenAsync();
                using (SqlDataReader reader =await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        var product = new GetProducts
                        {
                            Id = productId,
                            Name = reader["Name"].ToString(),
                            Quantity = (int)reader["Quantity"],
                            EAN = reader["EAN"].ToString()
                        };
                        if (product != null)
                        {
                            isExists = true;
                        }
                    }
                }
            }
            return isExists;
        }

        public async Task<IEnumerable<GetProducts>> GetAll()
        {
            string sqlGetById = "SELECT Id,Name,Quantity,EAN FROM Products WHERE ISNULL(Id,0) <> 0";
            List<GetProducts> productsList = new List<GetProducts>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(sqlGetById, connection);

                await connection.OpenAsync();
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var product = new GetProducts
                        {
                            Id = (int)reader["Id"],
                            Name = reader["Name"].ToString(),
                            Quantity = (int)reader["Quantity"],
                            EAN = reader["EAN"].ToString()
                        };
                        productsList.Add(product);
                    }
                }
            }
            return productsList;
        }
    }
}
