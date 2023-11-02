using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Reflection;

namespace ZooWeb.Pages.TicketSales
{
	public class EditModel : PageModel
	{
		public TicketSaleInfo info = new TicketSaleInfo();
		public string errorMsg = "";
		public string successMsg = "";
		public void OnGet()
		{
			String TicketSaleID = Request.Query["id"];
			// TODO: actually use this (addresses race condition)
			if (TicketSaleID == null || TicketSaleID == "") { errorMsg = "y tho?"; return; };

			string connectionString = "Server=tcp:zoowebdbserver.database.windows.net,1433;Database=ZooWeb_db;User ID=zooadmin;Password=peanuts420!;Trusted_Connection=False;Encrypt=True;";
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				String sql = "SELECT * FROM ticket_sales WHERE Ticket_ID=@TicketId";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.AddWithValue("@TicketID", TicketSaleID);
					using (SqlDataReader reader = command.ExecuteReader())
					{
						if (reader.Read())
						{
							info.TicketID = reader.GetInt32(0).ToString();
							info.PassType = reader.GetString(1);
							info.EmployeeID = reader.GetInt32(2).ToString();
							info.VisitorPn = reader.GetInt64(3).ToString();
							info.Date = reader.GetDateTime(4).ToString("yyyy-MM-dd");
							info.Total = reader.GetDecimal(5).ToString();
							info.ReceiptNumber = reader.GetInt64(6).ToString();

						}
					}
				}
			}
		}
		public void OnPost()
		{
			//must add check for null later
			info.TicketID = Request.Form["TicketID"];
			info.PassType = Request.Form["PassType"];
			info.EmployeeID = Request.Form["EmployeeID"];
			info.VisitorPn = Request.Form["VisitorPn"];
			info.Date = Request.Form["Date"];
			info.Total = Request.Form["Total"];
			info.ReceiptNumber = Request.Form["ReceiptNumber"];

			FieldInfo[] fields = info.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);

			foreach (FieldInfo field in fields)
			{
				object fieldValue = field.GetValue(info);
				if ((fieldValue == "" || fieldValue == null))
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
					string sql = "UPDATE ticket_sales " +
						"SET PassType=@PassType, EmployeeId=@EmployeeID, VisitorPn=@VisitorPn, Date=@Date, Total=@Total, ReceiptNumber=@ReceiptNumber" +
						"WHERE TicketId=@TicketID";

					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("@TicketId", int.Parse(info.TicketID));
						command.Parameters.AddWithValue("@PassType", long.Parse(info.PassType));
						command.Parameters.AddWithValue("@EmployeeId", short.Parse(info.EmployeeID));
						command.Parameters.AddWithValue("@VisitorPn", long.Parse(info.VisitorPn));
						command.Parameters.AddWithValue("@Date", info.Date);
						command.Parameters.AddWithValue("@Total", info.Total);
						command.Parameters.AddWithValue("@ReceiptNumber", info.ReceiptNumber);


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
			successMsg = "Ticket Sales Updated";

			Response.Redirect("/TicketSales/Index");
		}
	}
}
