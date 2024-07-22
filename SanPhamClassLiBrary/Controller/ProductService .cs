using SanPhamClassLiBrary.DBconnect;
using SanPhamClassLiBrary.Interface;
using SanPhamClassLiBrary.Model;
using SanPhamClassLiBrary.Validate;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SanPhamClassLiBrary.Controller
{
    public class ProductService : IProductService
    {
        public List<ProductModel> GetAllProducts(string searchTerm, string isActive)
        {
            var products = new List<ProductModel>();
            try
            {
                using (var conn = ConnectSQLSeverDB.GetSqlConnection())
                {
                    using (var command = new SqlCommand("GetAllProducts", conn))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@SearchTerm", string.IsNullOrEmpty(searchTerm) ? (object)DBNull.Value : searchTerm);
                        command.Parameters.AddWithValue("@IsActive", string.IsNullOrEmpty(isActive) ? (object)DBNull.Value : isActive);


                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var product = new ProductModel
                                {
                                    Id = (int)reader["Id"],
                                    Name = reader["Name"].ToString(),
                                    Description = reader["Description"].ToString(),
                                    IsActive = reader["StatusDescription"].ToString(),
                                    Price = (int)reader["Price"]
                                };
                                products.Add(product);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return products;
        }
        public ProductModel GetProductById(int id)
        {
            ProductModel product = null;
            try
            {
                using (var connection = ConnectSQLSeverDB.GetSqlConnection())
                {
                    /* string query = "SELECT * FROM Products WHERE Id = @Id";*/
                    using (var command = new SqlCommand("GetProductById", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Id", id);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                product = new ProductModel
                                {
                                    Id = (int)reader["Id"],
                                    Name = reader["Name"].ToString(),
                                    Description = reader["Description"].ToString(),
                                    IsActive = reader["IsActive"].ToString(),
                                    Price = (int)reader["Price"]
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return product;
        }
        public void AddProduct(ProductModel product)
        {
            try
            {
                // Kiểm tra dữ liệu sản phẩm
                if (ValidateSanPham.ValidateProduct(product, out string errorMessage))
                {
                    // Nếu dữ liệu hợp lệ, thực hiện thêm sản phẩm
                    using (var conn = ConnectSQLSeverDB.GetSqlConnection())
                    {
                        /* var query = "INSERT INTO Products (Name, Description, IsActive, Price) VALUES (@Name, @Description, @IsActive, @Price)";*/
                        using (var command = new SqlCommand("AddProduct", conn))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@Name", product.Name);
                            command.Parameters.AddWithValue("@Description", product.Description);
                            command.Parameters.AddWithValue("@IsActive", product.IsActive);
                            command.Parameters.AddWithValue("@Price", product.Price);
                            command.ExecuteNonQuery();
                        }
                    }
                }
                else
                {
                    // Xử lý thông báo lỗi
                    throw new ArgumentException(errorMessage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void DeleteProduct(int id)
        {
            try
            {
                using (var connection = ConnectSQLSeverDB.GetSqlConnection())
                {

                    using (var command = new SqlCommand("DeleteProduct", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Id", id);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void UpdateProduct(ProductModel product)
        {
            try
            {
                // Kiểm tra dữ liệu sản phẩm
                if (ValidateSanPham.ValidateProduct(product, out string errorMessage))
                {
                    // Nếu dữ liệu hợp lệ và sản phẩm tồn tại, thực hiện cập nhật sản phẩm
                    using (var conn = ConnectSQLSeverDB.GetSqlConnection())
                    {
                        /* var query = "UPDATE Products SET Name = @Name, Description = @Description, IsActive = @IsActive, Price = @Price WHERE Id = @Id";*/
                        using (var command = new SqlCommand("UpdateProduct", conn))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@Name", product.Name);
                            command.Parameters.AddWithValue("@Description", product.Description);
                            command.Parameters.AddWithValue("@IsActive", product.IsActive);
                            command.Parameters.AddWithValue("@Price", product.Price);
                            command.Parameters.AddWithValue("@Id", product.Id);
                            command.ExecuteNonQuery();
                        }
                    }
                }
                else
                {
                    // Xử lý thông báo lỗi
                    throw new ArgumentException(errorMessage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
