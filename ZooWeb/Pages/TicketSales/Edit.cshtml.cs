using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Reflection;
using System.Linq;

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
							info.SaleTotal = reader.GetSqlMoney(4).ToString();
							info.SaleDate = reader.GetDateTime(4).ToString();

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
			info.SaleTotal = Request.Form["Total"];
			//info.SaleDate = Request.Form["ReceiptNumber"];

			FieldInfo[] fields = info.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
			string[] excludedFields = { "TicketID", "SaleDate" };

			foreach (FieldInfo field in fields)
			{
				object fieldValue = field.GetValue(info);
				if (!excludedFields.Contains(field.Name) && (fieldValue == "" || fieldValue == null))
				{
					errorMsg = "All fields are required";
					System.Diagnostics.Debug.WriteLine(field);
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
						"SET PassType=@PassType, EmployeeId=@EmployeeId, VisitorPn=@VisitorPn, Sale_total=@SaleTotal" +
						" WHERE TicketId=@TicketId";

					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("@TicketId", info.TicketID);
						command.Parameters.AddWithValue("@PassType", info.PassType);
						command.Parameters.AddWithValue("@EmployeeId", info.EmployeeID);
						command.Parameters.AddWithValue("@VisitorPn", info.VisitorPn);
						command.Parameters.AddWithValue("@SaleTotal", info.SaleTotal);
						//command.Parameters.AddWithValue("@SaleDate", info.SaleDate);


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

			Response.Redirect("/Ticket Sales/Index");
		}
	}
}
