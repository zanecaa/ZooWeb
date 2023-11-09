using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Reflection;
using ZooWeb.Pages.Employees;

namespace ZooWeb.Pages.Revenue
{
    public class ReportModel : PageModel
    {
        public string errorMsg = "";
		public string successMsg = "";

		[DataType(DataType.Date)]
		public DateTime startDate { get; set; }
		public DateTime endDate { get; set; }
		public void OnGet()
        {
        }

		public List<revenueInfo> listRevenue = new List<revenueInfo>();
		public void OnPost() 
        {
			try
			{
				startDate = DateTime.Parse(Request.Form["start"]);
				endDate = DateTime.Parse(Request.Form["end"]);

				if (startDate > endDate)
				{
					throw new Exception("Start date cannot exceed end date.");
				}

				string connectionString = "Server=tcp:zoowebdbserver.database.windows.net,1433;Database=ZooWeb_db;User ID=zooadmin;Password=peanuts420!;Trusted_Connection=False;Encrypt=True;";

				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();
					string sql = "SELECT * FROM Revenue WHERE RevenueDate >= @StartDate AND RevenueDate <= @EndDate";

					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("@StartDate", startDate);
						command.Parameters.AddWithValue("@EndDate", endDate);

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
			catch (Exception ex)
			{
				if (ex.Message == "String '' was not recognized as a valid DateTime.")
				{
					errorMsg = "Dates cannot be empty.";
				}
				else
				{
					errorMsg = ex.Message;
				}
			}
		}
    }
}