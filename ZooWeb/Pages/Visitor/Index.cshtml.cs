﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Numerics;
namespace ZooWeb.Pages.Visitor
{
	[Authorize(Roles = "admin, zookeeper")]
	public class IndexModel : PageModel
	{
		public List<VisitorInfo> ListVisitors = new List<VisitorInfo>();

		public void OnGet()
		{
			string connectionString = "Server=tcp:zoowebdb.database.windows.net,1433;Database=ZooWeb_db;User ID=zooadmin;Password=peanuts420!;Trusted_Connection=False;Encrypt=True;";

			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				String sql = "SELECT * FROM visitor"; // Adjust the table name to match your database
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					using (SqlDataReader reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							VisitorInfo info = new VisitorInfo();
							info.PhoneNumber = reader.GetInt64(0).ToString();
							info.FirstName = reader.GetString(1);
							info.LastName = reader.GetString(2);
							info.BirthDate = reader.GetDateTime(3).ToString();

							ListVisitors.Add(info);
						}
					}
				}
			}
		}
	}

	public class VisitorInfo
	{
		public string FirstName;
		public string LastName;
		public string PhoneNumber;
		public string BirthDate;
	}
}
