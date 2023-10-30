using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ZooWeb.Pages.TicketSales
{
	public class IndexModel : PageModel
	{
		public List<TicketSaleInfo> ListTicketSales = new List<TicketSaleInfo>();

		public void OnGet()
		{
			string connectionString = "Server=tcp:yourdbserver.database.windows.net,1433;Database=YourDatabase;User ID=youruser;Password=yourpassword;Trusted_Connection=False;Encrypt=True;";

			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				String sql = "SELECT * FROM TicketSales";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					using (SqlDataReader reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							TicketSaleInfo info = new TicketSaleInfo();
							info.ReceiptNumber = reader.GetInt32(0).ToString();
							info.Date = reader.GetDateTime(1).ToString("yyyy-MM-dd");
							info.Total = reader.GetDecimal(2).ToString();
							info.SalesTax = reader.GetDecimal(3).ToString();
							info.TicketID = reader.GetInt32(4).ToString();
							info.PassType = reader.GetString(5);
							info.Essn = reader.GetInt32(6).ToString();

							ListTicketSales.Add(info);
						}
					}
				}
			}
		}
	}

	public class TicketSaleInfo
	{
		public string ReceiptNumber;
		public string Date;
		public string Total;
		public string SalesTax;
		public string TicketID;
		public string PassType;
		public string Essn;
	}
}
