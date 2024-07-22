using SanPhamClassLiBrary.DBconnect;
using SanPhamClassLiBrary.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SanPhamClassLiBrary.Validate
{
    public class ValidateSanPham
    {
        public static bool ValidateProduct(ProductModel product, out string errorMessage)
        {
            errorMessage = string.Empty;

            // Bước 1: Kiểm tra dữ liệu
            if (string.IsNullOrWhiteSpace(product.Name) || string.IsNullOrWhiteSpace(product.Description))
            {
                errorMessage = "Tên sản phẩm và mô tả không được để trống.";
                return false;
            }

            if (product.Price <= 0)
            {
                errorMessage = "Giá sản phẩm phải lớn hơn 0.";
                return false;
            }

            // Bước 2: Kiểm tra ký tự đặc biệt (giả sử không cho phép ký tự đặc biệt trong tên và mô tả)
            string specialCharPattern = @"[!@#$%^&*(),.?""':{}|<>]";
            if (Regex.IsMatch(product.Name, specialCharPattern) || Regex.IsMatch(product.Description, specialCharPattern))
            {
                errorMessage = "Tên sản phẩm và mô tả không được chứa ký tự đặc biệt.";
                return false;
            }

            // Bước 3: Kiểm tra sự tồn tại (Chỉ áp dụng cho cập nhật và xóa sản phẩm)
            if (product.Id > 0)
            {
                bool exists = CheckProductExists(product.Id);
                if (!exists)
                {
                    errorMessage = "Sản phẩm không tồn tại.";
                    return false;
                }
            }

            return true;
        }

        public static bool CheckProductExists(int id)
        {
            // Giả sử bạn có phương thức kiểm tra sự tồn tại trong cơ sở dữ liệu
            using (var connection = ConnectSQLSeverDB.GetSqlConnection())
            {
                string query = "SELECT COUNT(1) FROM Products WHERE Id = @Id";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    connection.Open();
                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }
    }
}
