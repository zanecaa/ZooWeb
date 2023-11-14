using Humanizer.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Numerics;
using ZooWeb.Controllers;

namespace ZooWeb.Pages.Revenue
{
    [Authorize(Policy = "admin")]
    public class IndexModel : PageModel
    {
        public List<revenueInfo> listRevenue = new List<revenueInfo>();
        protected readonly IConfiguration Config; public IndexModel(IConfiguration configuration){Config = configuration;}

		public void OnGet()
		{
			string connectionString = Config.GetConnectionString("connectionstring");

			try
			{
			using (SqlConnection connection = new SqlConnection(connectionString)) 
                { 
                    connection.Open();
                    String sql = "SELECT * FROM Revenue";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read()) 
                            { 
                                revenueInfo info = new revenueInfo();
                                info.Total = reader.GetDecimal(0).ToString();
                                info.ReceiptSource = reader.GetString(1);
							    info.ReceiptNum = reader.GetInt64(2).ToString();
                                info.RevenueDate = reader.GetDateTime(3).ToString();

                                listRevenue.Add(info);
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
               Console.WriteLine("Exception: " + ex.ToString());
            }
        }
    }

    public class revenueInfo
    {
        public string Total;
        public string ReceiptSource;
        public string ReceiptNum;
        public string RevenueDate;
        public string Eid;
	}
}