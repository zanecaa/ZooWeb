using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace ZooWeb.Pages.Visitor
{
	public class IndexModel : PageModel
	{
		public List<VisitorInfo> ListVisitors = new List<VisitorInfo>();

		public void OnGet()
		{
			string connectionString = "Server=tcp:yourdbserver.database.windows.net,1433;Database=YourDatabase;User ID=youruser;Password=yourpassword;Trusted_Connection=False;Encrypt=True;";

			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				String sql = "SELECT * FROM Visitors"; // Adjust the table name to match your database
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					using (SqlDataReader reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							VisitorInfo info = new VisitorInfo();
							info.Id = reader.GetInt32(0);
							info.FirstName = reader.GetString(1);
							info.LastName = reader.GetString(2);
							info.PhoneNumber = reader.GetString(3);
							info.BirthDate = reader.GetDateTime(4);

							ListVisitors.Add(info);
						}
					}
				}
			}
		}
	}

	public class VisitorInfo
	{
		public int Id;
		public string FirstName;
		public string LastName;
		public string PhoneNumber;
		public DateTime BirthDate;
	}
}
