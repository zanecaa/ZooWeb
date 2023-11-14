using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Reflection;

namespace ZooWeb.Pages.TicketSales
{
    public class CreateModel : PageModel
    {
        public TicketSaleInfo info = new TicketSaleInfo();
        public string errorMsg = "";
		public string successMsg = "";
        public void OnGet()
        {
        }

        public void OnPost() 
        {
			//must add check for null later
			info.TicketID = Request.Form["TicketID"];
			info.PassType = Request.Form["PassType"];
			info.EmployeeID = Request.Form["EmployeeID"];
			info.VisitorPn = Request.Form["VisitorPn"];
			info.ReceiptNumber = "placeholder";
			Decimal total = decimal.Parse(Request.Form["Total"]);


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
				string connectionStringTotal = "Server=tcp:zoowebdbserver.database.windows.net,1433;Database=ZooWeb_db;User ID=zooadmin;Password=peanuts420!;Trusted_Connection=False;Encrypt=True;";
				using (SqlConnection connection = new SqlConnection(connectionStringTotal))
				{
					connection.Open();
					string sql = "INSERT INTO receipt VALUES (@Date, @Total)";

					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("@Total", total);
						command.Parameters.AddWithValue("@Date", DateTime.Now);

						command.ExecuteNonQuery();
					}
				}

				string connectionStringID = "Server=tcp:zoowebdbserver.database.windows.net,1433;Database=ZooWeb_db;User ID=zooadmin;Password=peanuts420!;Trusted_Connection=False;Encrypt=True;";

				using (SqlConnection connection = new SqlConnection(connectionStringID))
				{
					connection.Open();
					String sql = "SELECT TOP 1 * FROM receipt ORDER BY ReceiptNumber DESC;";
					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						using (SqlDataReader reader = command.ExecuteReader())
						{
							while (reader.Read())
							{
								info.ReceiptNumber = reader.GetInt64(0).ToString();
							}
						}
					}
				}

				string connectionString = "Server=tcp:zoowebdbserver.database.windows.net,1433;Database=ZooWeb_db;User ID=zooadmin;Password=peanuts420!;Trusted_Connection=False;Encrypt=True;";
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();
					string sql = "INSERT INTO ticket_sales VALUES (@TicketId, @PassType, @EmployeeId, @VisitorPn, @ReceiptNumber)";

					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("@TicketID", int.Parse(info.TicketID));
						command.Parameters.AddWithValue("@PassType", info.PassType);
						command.Parameters.AddWithValue("@EmployeeID", long.Parse(info.EmployeeID));
						command.Parameters.AddWithValue("@VisitorPn", long.Parse(info.VisitorPn));
						command.Parameters.AddWithValue("@ReceiptNumber", long.Parse(info.ReceiptNumber));

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
			successMsg = "New Ticket Added";

			Response.Redirect("/TicketSales/Index");
		}
    }
}
