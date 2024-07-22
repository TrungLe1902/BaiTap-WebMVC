using SanPhamClassLiBrary.DBconnect;
using SanPhamClassLiBrary.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SanPhamClassLiBrary.Controller
{
    public class ProductStatusService
    {
        public List<ProductStatus> GetAllStatuses()
        {
            var statuses = new List<ProductStatus>();
            using (var conn = ConnectSQLSeverDB.GetSqlConnection())
            {
                var query = "SELECT StatusId, StatusName FROM ProductStatuses";
                using (var command = new SqlCommand(query, conn))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var status = new ProductStatus
                            {
                                StatusId = (int)reader["StatusId"],
                                StatusName = reader["StatusName"].ToString()
                            };
                            statuses.Add(status);
                        }
                    }
                }
            }
            return statuses;
        }
    }
}
