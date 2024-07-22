using SanPhamClassLiBrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SanPhamClassLiBrary.Interface
{
    public interface IProductService
    {
        List<ProductModel> GetAllProducts(string searchTerm , string isActive );
        ProductModel GetProductById(int id);
        void AddProduct(ProductModel product);
        void UpdateProduct(ProductModel product);
        void DeleteProduct(int id);
    }
}
