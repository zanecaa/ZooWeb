using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Data.SqlClient;
using System.Reflection;

namespace ZooWeb.Pages.AmenitySales
{
	public class CreateModel : PageModel
	{
		public AmenitytSalesInfo info = new AmenitytSalesInfo();
		public string errorMsg = "";
		public string successMsg = "";
		public void OnGet()
		{
		}

		public void OnPost()
		{

			info.EID = Request.Form["EID"];
			info.LocationID = Request.Form["LocationID"];
			info.SaleType = Request.Form["SaleType"];
			info.SaleDate = Request.Form["SaleDate"];
			info.Total = Request.Form["Total"];
			//info.ReceiptNumber = Request.Form["ReceiptNumber"];

			FieldInfo[] fields = info.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);

			foreach (FieldInfo field in fields)
			{
				object fieldValue = field.GetValue(info);
				if (field.Name != "ReceiptNumber" && (fieldValue == "" || fieldValue == null))
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

					string receipt_sql = "INSERT INTO receipt (Date, SaleTotal)" +
						"	VALUES (@SaleDate, @Total)";
					string get_receiptnum_sql = "SELECT ReceiptNumber FROM receipt WHERE Date=@SaleDate";
					//string amenity_sql = "INSERT INTO amenitySales VALUES (@EID, @LocationID, @SaleType, @SaleDate, @Total, @ReceiptNumber)";
					string amenity_sql = "INSERT INTO amenitySales (Eid, LocationID, SaleType, ReceiptNumber)" +
						"	VALUES (@EID, @LocationID, @SaleType, @ReceiptNumber)";
					
					using (SqlCommand command = new SqlCommand(receipt_sql, connection))
					{
						command.Parameters.AddWithValue("@SaleDate", info.SaleDate);
						command.Parameters.AddWithValue("@Total", info.Total);

						command.ExecuteNonQuery();
					}

					using (SqlCommand command = new SqlCommand(get_receiptnum_sql, connection))
					{
						command.Parameters.AddWithValue("@SaleDate",info.SaleDate);
						using (SqlDataReader reader = command.ExecuteReader())
						{
							if (reader.Read())
							{
								info.ReceiptNumber = reader.GetInt64(0).ToString();
							}
						}
					}
					
					using (SqlCommand command = new SqlCommand(amenity_sql, connection))
					{
						command.Parameters.AddWithValue("@EID", int.Parse(info.EID));
						command.Parameters.AddWithValue("@LocationID", int.Parse(info.LocationID));
						command.Parameters.AddWithValue("@SaleType", info.SaleType);
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
			successMsg = "New Amenity Sale Added";

			Response.Redirect("/AmenitySales/Index");
		}
	}
}
