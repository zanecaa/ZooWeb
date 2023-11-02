using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZooWeb.Pages.AmenitySales;
using System.Data.SqlClient;
using System.Reflection;

namespace ZooWeb.Pages.AmenitySales
{
    public class EditModel : PageModel
    {
		public AmenitytSalesInfo info = new AmenitytSalesInfo();
		public string errorMsg = "";
		public string successMsg = "";
		public void OnGet()
        {
			String LocationId = Request.Query["id"];
			if (LocationId == null || LocationId == "") { errorMsg = "y tho?"; return; };

			string connectionString = "Server=tcp:zoowebdbserver.database.windows.net,1433;Database=ZooWeb_db;User ID=zooadmin;Password=peanuts420!;Trusted_Connection=False;Encrypt=True;";
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				String sql = "SELECT * FROM amenitySales WHERE LocationID=@LocationId";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.AddWithValue("@LocationId", LocationId);
					using (SqlDataReader reader = command.ExecuteReader())
					{
						if (reader.Read())
						{
							info.EID = reader.GetInt32(0).ToString();
							info.LocationID = reader.GetInt32(1).ToString();
							info.SaleType = reader.GetString(2);
							info.SaleDate = reader.GetDateTime(3).ToString("yyyy-MM-dd");
							info.Total = reader.GetDecimal(4).ToString();
							info.ReceiptNumber = reader.GetInt64(5).ToString();
						}
					}
				}
			}
		}
		public void OnPost()
		{

			info.EID = Request.Form["Employee_ID"];
			info.LocationID = Request.Form["Location_ID"];
			info.SaleType = Request.Form["Sale_type"];
			info.SaleDate = Request.Form["Sale_date"];
			info.Total = Request.Form["Total"];
			info.ReceiptNumber = Request.Form["Receipt_number"];

			FieldInfo[] fields = info.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);

			foreach (FieldInfo field in fields)
			{
				object fieldValue = field.GetValue(info);
				if (fieldValue == "" || fieldValue == null)
				{
					errorMsg = "All fields are required";
					return;
				}
			}

			try
			{
				string connectionString = "Server=tcp:zoowebdbserver.database.windows.net,1433;Database=ZooWeb_db;User ID=zooadmin;Password=peanuts420!;Trusted_Connection=False;Encrypt=True;";
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();
					string sql = "UPDATE AmenitySales " +
						"SET Eid=@Eid, SaleType=@SaleType, SaleDate=@SaleDate, Total=@Total, ReceiptNumber=@ReceiptNumber " +
						"WHERE LocationID=@LocationId";
					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("@Eid", int.Parse(info.EID));
						command.Parameters.AddWithValue("@LocationID", int.Parse(info.LocationID));
						command.Parameters.AddWithValue("@SaleType", info.SaleType);
						command.Parameters.AddWithValue("@SaleDate", info.SaleDate);
						command.Parameters.AddWithValue("@Total", decimal.Parse(info.Total));
						command.Parameters.AddWithValue("@ReceiptNumber", int.Parse(info.ReceiptNumber));

						command.ExecuteNonQuery();
					}
				}
			}
			catch (Exception ex)
			{
				errorMsg = ex.Message;
				return;
			}


			foreach (FieldInfo field in fields)
			{
				field.SetValue(info, "");
			}
			successMsg = "Amenity Sale Updated";

			Response.Redirect("/AmenitySales/Index");
		}
	}
}
