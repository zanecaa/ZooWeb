﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ZooWeb.Pages.TicketSales
{
	[Authorize(Policy = "admin")]
	public class IndexModel : PageModel
	{
		public List<TicketSaleInfo> ListTicketSales = new List<TicketSaleInfo>();

		public void OnGet()
		{
			string connectionString = "Server=tcp:zoowebdbserver.database.windows.net,1433;Database=ZooWeb_db;User ID=zooadmin;Password=peanuts420!;Trusted_Connection=False;Encrypt=True;";

			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				String sql = "SELECT * FROM ticket_sales";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					using (SqlDataReader reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							TicketSaleInfo info = new TicketSaleInfo();
							info.TicketID = reader.GetInt32(0).ToString();
							info.PassType = reader.GetString(1);
							info.EmployeeID = reader.GetInt32(2).ToString();
							info.VisitorPn = reader.GetInt64(3).ToString();
							info.Date = reader.GetDateTime(4).ToString("yyyy-MM-dd");
							info.Total = reader.GetDecimal(5).ToString();
							info.ReceiptNumber = reader.GetInt64(6).ToString();

							ListTicketSales.Add(info);
						}
					}
				}
			}
		}
	}

	public class TicketSaleInfo
	{
		public string TicketID;
		public string PassType;
		public string EmployeeID;
		public string VisitorPn;
		public string Date;
		public string Total;
		public string ReceiptNumber;
	}
}
